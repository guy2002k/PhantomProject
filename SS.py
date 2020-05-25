import firebase_admin
import json
import socket
import time
import datetime
import os
import subprocess
import pathlib
import re


from firebase_admin import credentials
from firebase_admin import firestore
from enum import Enum,auto
from UnionEncryption import Wrapper
from Packet import Packet
from random import randint
from RSAEncryption import RSA_Class
from ClientEncryption import Tetris_Tor_Encryption
from NetworkSystem import Network_Properties
from threading import Thread,Lock



def tracking_server_alive(func:callable):
        def inner(*args, **kwargs):
            Thread(target=subprocess.call,args=(SlaveServer._SlaveServer__CHECK_SERVER_PATH,)).start()
            Thread(target=func,args=args,kwargs=kwargs).start()
        return  inner



def capsule_func(func:callable):        
        def inner(*args, **kwargs):
            mutex=SlaveServer.mutex
            mutex.acquire()

            thread_num=args[3]
            times_space=2

            print("Opening Thread number--> "+str(thread_num))
            for i in range(times_space):
                print()
            func(*args, **kwargs)
            for i in range(times_space):
                print()
            print("Closing Thread number--> "+str(thread_num))

            mutex.release()
            SlaveServer.threads.pop(0)
        return  inner        



class SlaveServer:
    
    class Clients(Enum):
        TOR_SERVER=auto(),
        TOR_CLIENT=auto(),
        CLIENT_TARGET=auto(),
        OTHER=auto()

    __CERTIFICATION_FILE_PATH=str(pathlib.Path().absolute()).replace(r'\\',r'\\\\')+"\\Jsons\\Tor-Certificate.json"
    __CHECK_SERVER_PATH=str(pathlib.Path().absolute()).replace(r'\\',r'\\\\')+"\\CheckServer\\dist\\CheckServer.exe"
    __MAX_BITS=2430

    threads=[]
    mutex=Lock()

    def __init__(self):
        firebase_admin.initialize_app(credentials.Certificate(self.__CERTIFICATION_FILE_PATH))
        self.__db=firestore.client()
        self.__rsa=RSA_Class()

        hostname=socket.gethostname()
        self.__PROPERTIES_TO_ME=Packet(Network_Properties.local_ip(),self.__get_new_port(),hostname=hostname)

        self.__set_new_fromMe_port() 

        self.__upload_data()
        #self.__tracking_server_alive()

    def __get_new_port(self) -> int:
        while True:
            try:
                port=randint(1,65535)
                socket.socket().bind(("127.0.0.1",port))
                break

            except:
                pass

        return port        

    def __set_new_fromMe_port(self,update:bool=False):
        port=self.__get_new_port()

        self.__PROPERTIES_FROM_ME=Packet(Network_Properties.local_ip(),port)

        if(update):
            self.__db.collection("Servers").document(self.__this_server_name).update({"PORTFROMME":port})       


    def __get_nic_name(self):
        nic_name=""
        external_ip=Network_Properties.external_ip()

        try:
            for i in self.__db.collection("Nics").stream():
                nic=i.to_dict()

                if(nic["IP"]==external_ip):
                    nic_name=i.id
                    break
                
            if(nic_name==""):    
                raise ValueError()

        except:
            nic_name="Network"+str(len(list(filter(lambda network_name: True if("Network" in network_name) else False, list(network.id for network in self.__db.collection("Nics").stream())))))
            self.__db.collection("Nics").document(nic_name).set({"IP":external_ip})   

        return nic_name


    def __upload_data(self):
        data=[]

        self.__this_server_name="ss"+str(len(list(filter(lambda server_name: True if("ss" in server_name) else False, list(server.id for server in self.__db.collection("Servers").stream())))))

        self.__nic_name=self.__get_nic_name()

        self.__db.collection ("Servers").document (self.__this_server_name).set(
        {"IP":self.__PROPERTIES_TO_ME.get (Packet.Options.IP),"PORTTOME":self.__PROPERTIES_TO_ME.get (Packet.Options.PORT),
        "PORTFROMME":self.__PROPERTIES_FROM_ME.get (Packet.Options.PORT),
        "HOSTNAME":self.__PROPERTIES_TO_ME.get (Packet.Options.HOSTNAME),"BUSY":False,"NIC":self.__nic_name})

        data.append({
                    "Pid":os.getpid(),
                    "Name":self.__this_server_name
                    })

        with open("./Jsons/ServersProp.json" ,'w') as server_prop_file:
                json.dump(data,server_prop_file)      


    def __who_is_the_client(self,ip:str,port:int):    #who is that pokemon'?
        for server in self.__db.collection("Servers").stream():
            slave_server=server.to_dict()
            if(slave_server["IP"]==ip and slave_server["PORTFROMME"]==port):
                print("!Is Tor Server!")
                print("IP--> "+slave_server["IP"])
                print("PORT--> "+str(slave_server["PORTFROMME"]))
                print("HOSTNAME--> "+slave_server["HOSTNAME"])
                return SlaveServer.Clients.TOR_SERVER

        for client in self.__db.collection("Clients").stream():
            client=client.to_dict()
            if(client["IPSRC"]==ip and client["PORTSRC"]==port):
                print("!Is Tor Client!")
                print("IP--> "+client["IPSRC"])
                print("PORT--> "+str(client["PORTSRC"]))
                print("HOSTNAME--> "+client["HOSTNAME"])
                return SlaveServer.Clients.TOR_CLIENT

            elif(client["IPDST"]==ip and client["PORTDST"]==port):
                print("!Is Tor Client's target!")
                print("IP--> "+client["IPDST"])
                print("PORT--> "+str(client["PORTDST"]))
                return SlaveServer.Clients.CLIENT_TARGET 

        print("!Is new Connection!")
        print("IP--> "+ip)
        print("PORT--> "+str(port))        
        return SlaveServer.Clients.OTHER


    def __get_target_owner(self,ip:str,port:int):
        owner=['',Packet('1.1.1.1',2),""]
        min_timestamp=datetime.datetime.now().timestamp()*1000000

        for client in self.__db.collection("Clients").stream():
            name=client.id
            client=client.to_dict()

            if(client["IPDST"]==ip and client["PORTDST"]==port and client["ACTIVE"]==True):
                if((client["TIMESTAMP"]>0) and (client["TIMESTAMP"]<min_timestamp)):
                    owner[0]=name
                    owner[1]=Packet(client["IPSRC"],client["PORTSRC"])
                    owner[2]=client["NIC"]
                    min_timestamp=client["TIMESTAMP"]

        try:
            print("!!!!!!!!!!!!!!!!!!OWNER IS "+owner[0]+"!!!!!!!!!!!!!!!!!!")             
            self.__db.collection("Clients").document(owner[0]).update({"TIMESTAMP":-1.0})

            if(owner[2]!=self.__nic_name):
                owner[1].set(((self.__db.collection("Nics").document(owner[2]).get().to_dict())["IP"]),Packet.Options.IP)  

            return owner[1]

        except:    
            raise Exception("friking little dicking targets owner")        



    def __sort_randomly(self,lst:list) -> list:
        new_lst=[]

        length=len(lst)

        while(length!=0):
            index=randint(0,length-1)
            new_lst.append(lst[index]) 
            lst.pop(index)

            length=len(lst)

        return new_lst                 


    def __create_trace(self) -> list:
        servers=[]

        while True:
            try:
               stops=randint(1,len(list(filter(lambda server_det: (not (server_det["BUSY"])), list(server.to_dict() for server in self.__db.collection("Servers").stream())))))
               break

            except: pass

        for server in self.__db.collection("Servers").stream():
            if(server.id!=self.__this_server_name):
                slave_server=server.to_dict()

                if(slave_server["BUSY"]): pass

                else:
                    ip=slave_server["IP"] if(slave_server["NIC"]==self.__nic_name) else (self.__db.collection("Nics").document(slave_server["NIC"]).get().to_dict())["IP"]

                    servers.append(Packet(ip,slave_server["PORTTOME"]))
                
                stops-=1

            if(stops==0): break        

        if(len(servers)==0):
            return None

        return self.__sort_randomly(servers) 


    def __send_to_tor_server(self,server:Packet,slave_client:socket.socket):
        request=server.get(Packet.Options.REQUEST)

        times_to_send=int(len(request)/self.__MAX_BITS)+1 if (len(request)%self.__MAX_BITS!=0) else int(len(request)/self.__MAX_BITS)

        slave_client.send(str(times_to_send).encode())

        if(slave_client.recv(1024).decode()!="got times"):
            raise Exception("Slave-server doesn't work")

        for i in range(1,times_to_send+1):
            slave_client.send((request[(i-1)*self.__MAX_BITS:i*self.__MAX_BITS]).encode())

        if(slave_client.recv(1024).decode()!="close socket"):
            raise Exception("Slave-server doesn't work")

        slave_client.close() 



    def __send_to_client(self,next_stop:Packet,reply:str,slave_client:socket.socket):
        parts=self.__rsa.get_msg_in_parts(reply)
        
        self.__rsa.modulus_length=(len(parts[0].encode()),False)

        slave_client.send("Giving Times".encode())

        if(slave_client.recv(1024).decode()!="Ready To Get Times"):
            raise Exception("Something went wrong in the sentment")

        slave_client.send(str(len(parts)).encode())  

        if(slave_client.recv(1024).decode()!="Ready To Get Length"):
            raise Exception("Something went wrong in the sentment")

        slave_client.send(str(len(parts[0].encode())).encode())

        try:
            self.__rsa.public_key=slave_client.recv(self.__rsa.modulus_length).decode()
            print(self.__rsa.public_key)
            
            for i in range(len(parts)):
                slave_client.send(self.__rsa.encrypt(parts[i]).encode())
                a=slave_client.recv(1024).decode()
                print(a)
                if(a!="Continue times"):
                    raise Exception("problem....")

            # encrypted_msg=self.__rsa.encrypt(reply)
            # slave_client.send(encrypted_msg.encode())
        except:
            raise Exception("Rsa doesn't work")

        slave_client.send("finish times".encode())

        if(slave_client.recv(1024).decode()!="got the massage!"):
            raise Exception("Slave-server doesn't work")

        slave_client.send("close socket".encode())
        slave_client.close()




    def __send_to_target(self,next_stop:Packet,slave_client:socket.socket):
        try:
            a=next_stop.get(Packet.Options.REQUEST).encode().decode("unicode_escape")
            next_stop.set(a, Packet.Options.REQUEST)

            #if the target doesn't reply back, aka google.com:80
            slave_client.settimeout(120)

            slave_client.connect((next_stop.get(Packet.Options.IP),next_stop.get(Packet.Options.PORT)))
            slave_client.sendall((next_stop.get(Packet.Options.REQUEST)).encode())

            try:
                reply=slave_client.recv(65536).decode()

            except:
                reply="Error 502: Destination has not response back"    

        except:
            reply="Error 0xb042f: Destination is unvailable"

        slave_client.close()

        return reply


    def __delete_client(self,client_prop:Packet):     
        for i in self.__db.collection("Clients").stream():
            client=i.to_dict()

            if(client["IPSRC"]==client_prop.get(Packet.Options.IP) and client["PORTSRC"]==client_prop.get(Packet.Options.PORT)):
                self.__db.collection("Clients").document(i.id).delete()
                break        



    @capsule_func
    def __clients_acceptor(self,conn:socket.socket,client_prop:tuple,number_client:int):
        slave_client=socket.socket()
        
        error_to_conn=False

        print("------------------------Client Number "+str(number_client)+"------------------------")

        client_type=self.__who_is_the_client(client_prop[0],client_prop[1])

        #FROM_SERVER-------------------------------------------------------------------------------------------------
        if(client_type==SlaveServer.Clients.TOR_SERVER):  #checks if the sender is tor server
           request="" 

           times=int(conn.recv(1024).decode())
           conn.send(("got times").encode())

           for i in range(times):
              request+=conn.recv(self.__MAX_BITS).decode()

           conn.send("close socket".encode())
           conn.close() 

           while True:    
                try:
                    next_stop=Wrapper.unwrap(request)
                    next_stop.set(Wrapper.request_scaler(next_stop.get(Packet.Options.REQUEST),False),Packet.Options.REQUEST)

                except:
                    try:
                        next_stop=Tetris_Tor_Encryption.decrypt(request)

                    except:
                        raise Exception("Why everybody went out?")    




                next_stop_type=self.__who_is_the_client(next_stop.get(Packet.Options.IP),next_stop.get(Packet.Options.PORT))
                
                try:
                    slave_client.bind(('',self.__PROPERTIES_FROM_ME.get(Packet.Options.PORT))) 
                    slave_client.connect((next_stop.get(Packet.Options.IP),next_stop.get(Packet.Options.PORT)))
                    error_to_conn=False

                except:
                    error_to_conn=True


                #TO_SERVER-------------------------------------------------------------------------------------------------
                if(next_stop_type==SlaveServer.Clients.TOR_SERVER):  #checks if the next stop is tor server           
                    if(not error_to_conn):
                        #sends to the tor server
                        self.__send_to_tor_server(next_stop,slave_client)
                        break
                #-------------------------------------------------------------------------------------------------
                    

                #TO_CLIENT-------------------------------------------------------------------------------------------------
                elif(next_stop_type==SlaveServer.Clients.TOR_CLIENT):
                    if(not error_to_conn):
                        try:
                            reply=Tetris_Tor_Encryption.encrypt(next_stop)


                            self.__send_to_client(next_stop,reply,slave_client)

                        except:
                            error_to_conn=True


                    if(error_to_conn):
                        self.__delete_client(next_stop)
                        print("Client disconected :(")    
                    
                    break

                    #remove client from db

                #-------------------------------------------------------------------------------------------------  



                #-------------------------------------------------------------------------------------------------    
                elif(next_stop_type==SlaveServer.Clients.CLIENT_TARGET):
                    #TO_TARGET---------------------------------------------------------------------------------
                    servers=self.__create_trace()

                    reply=self.__send_to_target(next_stop,slave_client)

                    #-------------------------------------------------------------------------------------------------

                    #{{{-------------------------------------------------------------------------------------------------
                    slave_client=socket.socket()    
                                
                    next_stop=self.__get_target_owner(next_stop.get(Packet.Options.IP),next_stop.get(Packet.Options.PORT))
                    next_stop.set(reply,Packet.Options.REQUEST)

                    reply=Tetris_Tor_Encryption.encrypt(next_stop)

                    while True:
                        #TO_SERVER-------------------------------------------------------------------------------------------------
                        if(servers is not None):
                            first_server=servers.pop(0)
                            first_server.set(Wrapper.wrap(len(servers),servers,reply),Packet.Options.REQUEST)

                            self.__set_new_fromMe_port(True)
                            try: 
                                slave_client.bind(('',self.__PROPERTIES_FROM_ME.get(Packet.Options.PORT))) 
                                slave_client.connect((first_server.get(Packet.Options.IP),first_server.get(Packet.Options.PORT)))

                                #sends to the tor server
                                self.__send_to_tor_server(first_server,slave_client)

                                break

                            except:
                                pass    
                        #-------------------------------------------------------------------------------------------------


                        #TO_CLIENT-------------------------------------------------------------------------------------------------    
                        else:

                            try: 
                                slave_client.connect((next_stop.get(Packet.Options.IP),next_stop.get(Packet.Options.PORT)))


                                self.__send_to_client(next_stop,reply,slave_client)

                            except:
                                self.__delete_client(next_stop)
                                print("Client disconected :(")
                                pass

                            finally:
                                break

                        #-------------------------------------------------------------------------------------------------    

                    break         

                    #-------------------------------------------------------------------------------------------------}}}
                    

                else:
                    raise Exception("Problem or Hacker Fucker") 

                    #-------------------------------------------------------------------------------------------------
        #-------------------------------------------------------------------------------------------------

        #FROM_CLIENT-------------------------------------------------------------------------------------------------
        elif(client_type==SlaveServer.Clients.TOR_CLIENT):

            #-------------------------------------------------------------------------------------------
            #server-side
            if(conn.recv(1024).decode()!="Giving Times"):
                raise Exception("Something went wrong...")

            conn.send("Ready To Get Times".encode())

            times=int(conn.recv(1024).decode())

            conn.send("Ready To Get Length".encode())

            try:
                self.__rsa.modulus_length=(int(conn.recv(1024).decode()),True)
                print(self.__rsa.public_key)
                conn.send(self.__rsa.public_key.encode())

            except:
                raise Exception("There is a problem in the modulus rsa, or in send public")

            msg=""
            try:
                for i in range(times):
                    msg+=self.__rsa.decrypt(conn.recv(1024).decode())
                    conn.send("Continue times".encode())

                if (conn.recv (1024).decode () != "finish times"):
                    raise Exception ("no finish")

                print(msg)
                conn.send("got the massage!".encode())
                # msg=self.__rsa.decrypt(conn.recv(1024).decode())
                # conn.send("got the massage!".encode())

            except:
                raise Exception("problem with your decription, ya!")

            if(conn.recv(1024).decode()!="close socket"):
                   raise Exception("Slave-server doesn't work")
            
            conn.close()   
            #-------------------------------------------------------------------------------------------------


            #------------------------------------------------------------------------------------------
            servers=self.__create_trace()

            while True:
                if(servers is not None):
                    #TO_SERVER-------------------------------------------------------------------------------------------------
                    first_server=servers.pop(0)
                    first_server.set(Wrapper.wrap(len(servers),servers,msg),Packet.Options.REQUEST)

                    self.__set_new_fromMe_port(True)

                    try: 
                        slave_client.bind(('',self.__PROPERTIES_FROM_ME.get(Packet.Options.PORT))) 
                        slave_client.connect((first_server.get(Packet.Options.IP),first_server.get(Packet.Options.PORT)))

                        #sends to the tor server
                        self.__send_to_tor_server(first_server,slave_client)

                        break

                    except:
                        pass
                    #-------------------------------------------------------------------------------------------------

                else:
                    #TO_TARGET-------------------------------------------------------------------------------------------------
                    #connect to target
                    next_stop=Tetris_Tor_Encryption.decrypt(msg)


                    reply=self.__send_to_target(next_stop,slave_client)

                    #-------------------------------------------------------------------------------------------------

                    
                    #TO_CLIENT-------------------------------------------------------------------------------------------------    
                    #connect to client
                    #client-side
                    slave_client=socket.socket()                
                    next_stop=self.__get_target_owner(next_stop.get(Packet.Options.IP),next_stop.get(Packet.Options.PORT))
                    next_stop.set(reply,Packet.Options.REQUEST)

                    try:
                        slave_client.connect((next_stop.get(Packet.Options.IP),next_stop.get(Packet.Options.PORT))) 
                        
                        reply=Tetris_Tor_Encryption.encrypt(next_stop)

                        #sends to the client
                        self.__send_to_client(next_stop,reply,slave_client)

                    except:
                        self.__delete_client(next_stop)
                        print("Client disconected :(")
                        pass

                    finally:
                        break
                    #-------------------------------------------------------------------------------------------------
        #-------------------------------------------------------------------------------------------------


        else:
            #hackerrrr
            pass


        print("------------------------Finished Client Number "+str(number_client)+"------------------------")


    @tracking_server_alive
    def run_server(self):
        counter_client=0

        slave_server=socket.socket()

        slave_server.bind((self.__PROPERTIES_TO_ME.get(Packet.Options.IP),self.__PROPERTIES_TO_ME.get(Packet.Options.PORT)))
        slave_server.listen(5)

        while True:
          conn,client_prop=slave_server.accept()
          counter_client+=1

          self.threads.append(Thread(target=self.__clients_acceptor, args=(conn,client_prop,counter_client,)).start())

            
            


if __name__=="__main__":
    slavik=SlaveServer()
    slavik.run_server()
