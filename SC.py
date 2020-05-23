import firebase_admin
import json
import socket
import datetime
import time
import psutil
import pathlib
import os,sys


from firebase_admin import credentials
from firebase_admin import firestore
from RSAEncryption import RSA_Class
from Packet import Packet
from random import randint
from ClientEncryption import Tetris_Tor_Encryption
from NetworkSystem import Network_Properties
from os import listdir
from os.path import isfile, join
from threading import Thread,Lock


class SecureClient():

    __CERTIFICATION_FILE_PATH="\\Jsons\\Tor-Certificate.json"
    __CLIENTREQUEST_FILE_PATH="\\Jsons\\ClientRequest.json"
    __CLIENTNAME_FILE_PATH="\\Jsons\\ClientName.json"
    __REPLY_FLODER_PATH="\\Reply"


    def __init__(self):
        #paths
        path=SecureClient.__get_path()

        self.__CERTIFICATION_FILE_PATH = path + self.__CERTIFICATION_FILE_PATH
        self.__CLIENTREQUEST_FILE_PATH = path + self.__CLIENTREQUEST_FILE_PATH
        self.__CLIENTNAME_FILE_PATH = path + self.__CLIENTNAME_FILE_PATH
        self.__REPLY_FLODER_PATH = path + self.__REPLY_FLODER_PATH

        print("set_paths")

        firebase_admin.initialize_app(credentials.Certificate(self.__CERTIFICATION_FILE_PATH))
        
        self.__db=firestore.client()
        self.__rsa=RSA_Class()

        hostname=socket.gethostname()
        self.__PROPERTIES_SRC=Packet(Network_Properties.local_ip(),self.__get_new_src_port(),hostname=hostname)

        self.__this_client_name=""

    @staticmethod
    def __get_path():
        names=[]
        path = str(pathlib.Path().absolute()).replace(r"\\","\\\\")

        
        while(True):
            names=[name for name in os.listdir(path)]
            
            if("Jsons" in names):
                break

            elif(path==""):
                raise Exception("WHY YOU ARE ERASE JSONS FOLDER?")

            parts=path.split("\\")
            parts.pop(len(parts)-1)
            path="\\".join(parts)

        return path                 


    def __get_dest_conn(self):      
        while True:
            try:

                try:
                    with open(self.__CLIENTREQUEST_FILE_PATH,'r') as prop_file:
                        properties=json.load(prop_file)

                except:
                    raise Exception("You can not write in hebrew")  

                ip=properties[0]["Ip"][0]
                port=properties[0]["Port"][0]
                request=properties[0]["Request"][0]

                try:
                    self.__PROPERTIES_DST=Packet(ip,port)

                except:
                    print("Dns is--> "+ip)
                    self.__PROPERTIES_DST=Packet(socket.getaddrinfo(ip,port)[0][4][0],port)    

                properties[0]["Ip"].pop(0)
                properties[0]["Port"].pop(0)
                properties[0]["Request"].pop(0)

                with open(self.__CLIENTREQUEST_FILE_PATH ,'w') as prop_file:
                    json.dump(properties,prop_file)

                break

            except: pass    

        return request


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
        if(self.__this_client_name == ""):
            self.__this_client_name="sc"+str(len(list(filter(lambda client_name: True if("sc" in client_name) else False, list(client.id for client in self.__db.collection("Clients").stream())))))
        #self.__db.collection("Clients").document(self.__this_client_name).set({"IPSRC":self.__PROPERTIES_SRC.get(Packet.Options.IP),"PORTSRC":self.__PROPERTIES_SRC.get(Packet.Options.PORT),"IPDST":self.__PROPERTIES_DST.get(Packet.Options.IP),"PORTDST":self.__PROPERTIES_DST.get(Packet.Options.PORT),"HOSTNAME":self.__PROPERTIES_SRC.get(Packet.Options.HOSTNAME)})#

        print("!!!! I'm "+self.__this_client_name)

        self.__nic_name=self.__get_nic_name()

        self.__db.collection("Clients").document(self.__this_client_name).set(
            {"IPSRC":self.__PROPERTIES_SRC.get(Packet.Options.IP),"PORTSRC":self.__PROPERTIES_SRC.get(Packet.Options.PORT),
             "IPDST":self.__PROPERTIES_DST.get(Packet.Options.IP),"PORTDST":self.__PROPERTIES_DST.get(Packet.Options.PORT),
             "HOSTNAME":self.__PROPERTIES_SRC.get(Packet.Options.HOSTNAME),"ACTIVE":True,"TIMESTAMP":-1.0,"NIC":self.__nic_name})

        with open(self.__CLIENTNAME_FILE_PATH ,'w') as prop_file:
                json.dump([{"Name":self.__this_client_name}],prop_file)     

    def __disable_destenation(self):
        self.__db.collection("Clients").document(self.__this_client_name).update({"ACTIVE":False,"TIMESTAMP":-1.0})

    def __enable_destenation(self):
        self.__db.collection("Clients").document(self.__this_client_name).update({"ACTIVE":True,"TIMESTAMP":-1.0})

    def __sent_to_tor(self):
        self.__db.collection("Clients").document(self.__this_client_name).update({"TIMESTAMP":datetime.datetime.now().timestamp()})



    def __choose_server(self) -> Packet:
        valid_servers=[]

        while(len(valid_servers)==0):
            for i in self.__db.collection("Servers").stream():
                sevrer=i.to_dict()

                if(not sevrer["BUSY"]):
                    valid_servers.append(i.id)

            try:
                server_name=str(valid_servers[randint(0,len(valid_servers))])
                server=((self.__db.collection("Servers").document(server_name)).get()).to_dict()

            except:
                valid_servers=[]


        if(server["NIC"]==self.__nic_name):
            ip=server["IP"]

        else:
            ip=(self.__db.collection("Nics").document(server["NIC"]).get().to_dict())["IP"]            
        
        print("----------------------------Chose "+server_name+"----------------------------")
        print("IP--> "+ip)
        print("PORT--> "+str(server["PORTTOME"]))
        print("HOSTNAME--> "+server["HOSTNAME"])

        return Packet(ip,server["PORTTOME"])

    @property
    def destenation_properties(self):
        return (self.__PROPERTIES_DST.get(Packet.Options.IP),self.__PROPERTIES_DST.get(Packet.Options.PORT))

    @destenation_properties.setter
    def destenation_properties(self,properties:tuple):
        self.__PROPERTIES_DST.set(properties[0],Packet.Options.IP)  
        self.__PROPERTIES_DST.set(properties[1],Packet.Options.PORT)
        self.__upload_data()

    def __restart_sockets(self):
        self.__secure_client=socket.socket()
        self.__secure_client_server=socket.socket()

    def __get_new_src_port(self):
        while True:
            try:
                port=randint (1,65535)
                socket.socket ().bind (("127.0.0.1",port))
                break

            except:
                pass

        return port

    def __store_in_file(self,reply:str):
        number=0
        
        file_format = ".html" if("html" in reply) else ".txt"
        file_name = str(self.__PROPERTIES_DST.get(Packet.Options.IP)) + " " + str(self.__PROPERTIES_DST.get(Packet.Options.PORT))

        files = [f for f in listdir(self.__REPLY_FLODER_PATH) if isfile(join(self.__REPLY_FLODER_PATH, f))]

        for filee in files:
            if(file_name in filee):
                number+=1

        file_name = self.__REPLY_FLODER_PATH + "\\" + file_name + " " + str(number) + file_format

        writer=open(file_name,'w')
        writer.write(reply)
        writer.close()        


    def run_client(self): #put this in loop in the ui
        parts=[]


        request=self.__get_dest_conn()
        self.__upload_data()

        #refresh the start    
        self.__enable_destenation()  


        self.__PROPERTIES_DST.set(request,Packet.Options.REQUEST)
        request=Tetris_Tor_Encryption.encrypt(self.__PROPERTIES_DST)#changeeeeeeeeeeeeeeeeeeeeeeeeee!!!!!!!!!!!!!!!!!!!!!!
        


        parts=self.__rsa.get_msg_in_parts(request)

        print(parts)



        self.__rsa.modulus_length=(len(parts[0].encode()),False)

        print(self.__rsa.modulus_length)

        #client side    
        server=self.__choose_server()

        self.__restart_sockets()

        self.__secure_client.bind(('',self.__PROPERTIES_SRC.get(Packet.Options.PORT)))
        self.__secure_client.connect((server.get(Packet.Options.IP),server.get(Packet.Options.PORT)))

        self.__secure_client.send("Giving Times".encode())

        if(self.__secure_client.recv(1024).decode()!="Ready To Get Times"):
            raise Exception("Something went wrong in the sentment")

        self.__secure_client.send(str(len(parts)).encode())

        if(self.__secure_client.recv(1024).decode()!="Ready To Get Length"):
            raise Exception("Something went wrong in the sentment")
        

        self.__secure_client.send(str(len(parts[0].encode())).encode())

        try:
            self.__rsa.public_key=self.__secure_client.recv(self.__rsa.modulus_length).decode()
            print(self.__rsa.public_key)

            for i in range(len(parts)):
                self.__secure_client.send(self.__rsa.encrypt(parts[i]).encode())
                if(self.__secure_client.recv(1024).decode()!="Continue times"):
                    raise Exception("Problemmmm.....")

            # encrypted_msg=self.__rsa.encrypt(request)
            # self.__secure_client.send(encrypted_msg.encode())

        except:
            raise Exception("Rsa doesn't work") 

        self.__secure_client.send("finish times".encode())

        if(self.__secure_client.recv(1024).decode()!="got the massage!"):
            raise Exception("Slave-server doesn't work")



        #notify he sent to the tor server
        self.__sent_to_tor()



        self.__secure_client.send("close socket".encode())
        self.__secure_client.shutdown(socket.SHUT_RDWR)
        self.__secure_client.close()

        #server side
        self.__secure_client_server.bind((self.__PROPERTIES_SRC.get(Packet.Options.IP),self.__PROPERTIES_SRC.get(Packet.Options.PORT)))
        self.__secure_client_server.listen(5)

        reply,_=self.__secure_client_server.accept()


        if(reply.recv(1024).decode()!="Giving Times"):
            raise Exception("Something went wrong...")

        reply.send("Ready To Get Times".encode())

        times=int(reply.recv(1024).decode())

        print("times--> "+str(times))

        reply.send("Ready To Get Length".encode())

        try:
            self.__rsa.modulus_length=(int(reply.recv(1024).decode()),True)
            print(self.__rsa.public_key)
            reply.send(self.__rsa.public_key.encode())
        except:
            raise Exception("Something wrong in the recv or the send, check it")    

        reply_msg=""
        try:
            for i in range(times):
                reply_msg+=self.__rsa.decrypt(reply.recv(1024).decode())
                reply.send("Continue times".encode())

            if(reply.recv(1024).decode()!="finish times"):
                raise Exception("no finish")

            reply_msg=Tetris_Tor_Encryption.decrypt(reply_msg)    
            reply.send("got the massage!".encode())
        except:
            raise Exception("Rsa doesn't work")

        if (reply.recv(1024).decode() != "close socket"):
            raise Exception("Slave-server doesn't work")

        self.__secure_client_server.close()
        reply.close()

        #refresh the end
        self.__PROPERTIES_SRC.set(self.__get_new_src_port() ,Packet.Options.PORT)
        self.__disable_destenation()

        self.__store_in_file(reply_msg.get(Packet.Options.REQUEST))


if __name__=="__main__":
    # dst_ip="34.76.80.113"
    # dst_port=3213

    # dst_ip="web.whatsapp.com"
    # dst_port=80

    # dst_ip=socket.getaddrinfo(dst_ip,dst_port)[0][4][0]


    # client=SecureClient(dst_ip,dst_port)
    # print(client.run_client("who are you?"))

    client=SecureClient()

    while True:
      client.run_client() 
