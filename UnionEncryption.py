'''
This class will wrap and unwrap the packages
'''

import random
from Packet import *

class Wrapper:
    __MIN_CHARS=33
    __MAX_CHARS=127
    __MIN_LENGTH=386
    __MAX_LENGTH=480 #min+94 you need to have it!!!!!!!!!!!!!!!!!!

    #

    @staticmethod
    def request_scaler(request:str,to_encrypt=True):
        index=1

        request=list(request)

        if(to_encrypt):
            for i in range(len(request)):
                placer=int(ord(request[index-1])/(Wrapper._Wrapper__MAX_CHARS-Wrapper._Wrapper__MIN_CHARS))
                
                if((placer>0) or (int(ord(request[index-1]))>=Wrapper._Wrapper__MIN_CHARS)):
                    placer+=1
                
                placer+=Wrapper._Wrapper__MIN_CHARS
                request.insert(index,chr(placer))
                index+=2

        else:
            for i in range(int(len(request)/2)):
                placer=ord(request[index])-Wrapper._Wrapper__MIN_CHARS-1
                
                if((int(ord(request[index-1]))<(Wrapper._Wrapper__MAX_CHARS-Wrapper._Wrapper__MIN_CHARS)) or (placer<1)):
                   request[index-1]=chr(int(ord(request[index-1]))+(Wrapper._Wrapper__MAX_CHARS-Wrapper._Wrapper__MIN_CHARS)*placer)

                else:
                    request[index-1]=chr((int(ord(request[index-1]))-Wrapper._Wrapper__MAX_CHARS+Wrapper._Wrapper__MIN_CHARS)+(Wrapper._Wrapper__MAX_CHARS-Wrapper._Wrapper__MIN_CHARS)*placer)   
               
                request.pop(index)
                index+=1

        return ''.join(request)        


    @staticmethod
    def __scale(num:int,key:int,forward=True,is_length=False):
        min_scale=0
        max_scale=0

        scale=Wrapper._Wrapper__MAX_CHARS-Wrapper._Wrapper__MIN_CHARS

        if(is_length):
           min_scale=Wrapper._Wrapper__MIN_LENGTH
           max_scale=Wrapper._Wrapper__MAX_LENGTH
        else:
           min_scale=Wrapper._Wrapper__MIN_CHARS
           max_scale=Wrapper._Wrapper__MAX_CHARS

        if(forward):
            num=num+key
            if(num<min_scale or num>=max_scale):
                sign= 1 if num>0 else -1
                num=abs(num)
                mulitplier=int(num/scale)
                a=(num-scale*mulitplier+min_scale)
                return a
            else:
               return num

        else:
            st_real_remainder=str((num-Wrapper._Wrapper__MIN_CHARS)/scale)
            real_remainder=int(st_real_remainder.split(".")[1][0:10])

            for possible_num in range(key+min_scale,key+max_scale):
                st_possible_remainder=str(possible_num/scale-int(possible_num/scale))
                possible_remainder=int(st_possible_remainder.split(".")[1][0:10])

                if(real_remainder==possible_remainder):
                    return int(possible_num-key)

            raise Exception("fuckhhhhhhhhh")        



    @staticmethod
    def __key_maker(length=0,addition=0):
        if(length==0):
            length=random.randint(Wrapper._Wrapper__MIN_LENGTH+addition,Wrapper._Wrapper__MAX_LENGTH)

        key=""

        for i in range(0,length):
            key+=chr(random.randint(Wrapper._Wrapper__MIN_CHARS,Wrapper._Wrapper__MAX_CHARS))

        return key


    @staticmethod    
    def __vingenere_chiper(st:str,key:int,normal=True,times=1):
        new_st=""
        is_length=False

        if(len(st)==1):
            is_length=True

        for i in st:
            if(key>0 or normal):
               new_st+=chr(Wrapper._Wrapper__scale(ord(i),key*times,is_length=is_length))

            else: 
               new_st+=chr(Wrapper._Wrapper__scale(ord(i),-(key*times),False,is_length=is_length))     

        return new_st  

    
    @staticmethod
    def wrap(layers:int,packets:list,request:str) -> str:
        if(layers<1):
            return request

        ip=packets[layers-1].get(Packet.Options.IP)
        port=str(packets[layers-1].get(Packet.Options.PORT))

        while(len(port)!=len(ip)):
            port+=";"

        key_limit=Wrapper._Wrapper__key_maker(addition=len(ip))
        key_intermidate=Wrapper._Wrapper__key_maker(len(key_limit)-len(ip))

        print("key_limit-->"+str(len(key_limit)))
        print("key_intermidate-->"+str(len(key_intermidate)))

        request=key_limit+Wrapper._Wrapper__vingenere_chiper(ip,len(ip))+key_intermidate+Wrapper._Wrapper__vingenere_chiper(port,len(port))+Wrapper._Wrapper__vingenere_chiper(key_intermidate,len(key_intermidate))+Wrapper._Wrapper__vingenere_chiper(request,len(key_limit)+len(key_intermidate))+Wrapper._Wrapper__vingenere_chiper(key_limit,len(key_limit))+chr(len(key_intermidate))+chr(len(key_limit))    

        return Wrapper.wrap(layers-1,packets,request)

    @staticmethod
    def unwrap(request:str,layers=1,packets=[],sum_lengths=0) -> Packet:
        if(layers<1):
           return packets[len(packets)-1]
         
        key_limit_length=ord(request[len(request)-1])
        key_intermidate_length=ord(request[len(request)-2])


        key_limits=[request[0:key_limit_length]]
        key_limits.append(Wrapper._Wrapper__vingenere_chiper(key_limits[0],key_limit_length))

        for key_limit in key_limits:
            request=request.replace(key_limit,"")

        key_intermidates=[request[key_limit_length-key_intermidate_length:key_limit_length]] 
        key_intermidates.append(Wrapper._Wrapper__vingenere_chiper(key_intermidates[0],key_intermidate_length))

        for key_intermidate in key_intermidates:
            request=request.replace(key_intermidate,"🙃🐧❤️מיכל")

        details=request.split("🙃🐧❤️מיכל")

        for i in range(0,2):
            details[i]=Wrapper._Wrapper__vingenere_chiper(details[i],-(key_limit_length-key_intermidate_length))

        details[1]=int(details[1].replace(";",""))
        details[2]=details[2][0:len(details[2])-2]

        if(layers>1):
            st_key_limit_length=details[2][len(details[2])-1]
            st_key_intermidate_length=details[2][len(details[2])-2]
            details[2]=details[2][0:len(details[2])-2]

            st_key_limit_length=Wrapper._Wrapper__vingenere_chiper(st_key_limit_length,-(key_limit_length+key_intermidate_length),False)
            #print("key_limit-->"+str(ord(st_key_limit_length)))
            st_key_intermidate_length=Wrapper._Wrapper__vingenere_chiper(st_key_intermidate_length,-(key_limit_length+key_intermidate_length),False)
            #print("key_key_intermidate-->"+str(ord(st_key_intermidate_length)))
            details[2]=Wrapper._Wrapper__vingenere_chiper(details[2],-(key_limit_length+key_intermidate_length),False)+st_key_intermidate_length+st_key_limit_length

        else:
            details[2]=Wrapper._Wrapper__vingenere_chiper(details[2],-(key_limit_length+key_intermidate_length),False)


        packets.append(Packet(details[0],details[1],details[2]))

        return Wrapper.unwrap(details[2], layers-1,packets, key_limit_length+key_intermidate_length)


    # class Certificate:

    #     @staticmethod
    #     def encode(detail:str,Max:int):
    #        key_limit= 

    #     @staticmethod
    #     def decode():
            



        
            
            
if __name__=="__main__":
    packets=[Packet("1.1.1.1",4320),Packet("2.2.2.2",1690),
             Packet("3.3.3.3",3339),Packet("4.4.4.4",2360),
             Packet("5.5.5.5",4320),Packet("5.5.5.5",1111),
             Packet("1.1.1.1",4320),Packet("2.2.2.2",1690),
             Packet("3.3.3.3",3339),Packet("4.4.4.4",2360),
             Packet("5.5.5.5",4320),Packet("5.5.5.5",1111)]

    packet=[Packet("34.76.80.113",3213,"whats-app")]         

    print("Mangistu are you ready to be amazed? PLease press any key")
    input()         

    print("--------------------------------------WRAPP IT ON------------------------------------------------------------------")
    #a=Wrapper.wrap(len(packets),packets,(Wrapper.request_scaler("מזל טוב סבא!!! :("[::-1])))
    a=Wrapper.wrap(len(packets),packets,Wrapper.request_scaler("whats-app"))
    print(a)
    print("--------------------------------------WRAPP IT OFF------------------------------------------------------------------")

    input()




    print("--------------------------------------UNWRAPP IT ON------------------------------------------------------------------")
    b=Wrapper.unwrap(a)
    print(b.get(Packet.Options.IP))
    print(b.get(Packet.Options.PORT))
    print(Wrapper.request_scaler(b.get(Packet.Options.REQUEST),False))
    print("--------------------------------------UNWRAPP IT OFF------------------------------------------------------------------")

