import base64
import codecs
import math
import binascii
import re

from Crypto.PublicKey import RSA
from Crypto.Cipher import PKCS1_OAEP

class RSA_Class:

    def __init__(self,modulus_length=2048):
        self.__modulus_length=modulus_length
        self.__keys=self.__genarate_keys()

    def __genarate_keys(self):
        private_key=RSA.generate(self.__modulus_length)
        return (private_key.publickey(),private_key)

    @property
    def public_key(self):
        return (self.__keys[0]).export_key("PEM").decode()

    @public_key.setter
    def public_key(self,value:str):
        self.__keys=(RSA.import_key(value.encode()),self.__keys[1])

    @property
    def modulus_length(self) -> int:
        return self.__modulus_length



    def get_msg_in_parts(self,msg:str) -> list:
        massages=[]

        bits=msg.encode()
        length=int(len(bits))
        i=-1

        while(True):
            self.modulus_length=(length/(2**i),False)
            i+=1

            if(self.__modulus_length<=2048):
                break

            

        length=int(len(bits)/(i+1))+1
        
        for j in range(1,i+2):
            a=str(bits[length*(j-1):length*j])
            a=a[2:len(a)-1]
            massages.append(a)

        return massages      

     


    @modulus_length.setter
    def modulus_length(self,modulus_prop:(int,bool)):
        '''
        It calculates the bits that needs to be and the KEYS,
        that you want to use, it chackes limits by the formula 64*(2**n-1)
        that will calculate the perfect amount of bits that your key needs to have.
        Enjoy!!!

        Created by Guy Klerman yud beth 2, the favorite student of my programing teacher
        '''

        modulus_length,to_refresh_keys=modulus_prop

        n=int(modulus_length/2)-43

        for i in range(0,7):
            limit=64*(2**i-1)
            
            if(n<=limit):
                self.__modulus_length=2**(10+i)
                break
                
        print("modulus_length--> "+str(self.__modulus_length))
        
        if(to_refresh_keys):
            self.refresh()



    def refresh(self):
        self.__keys=self.__genarate_keys()                 

    def encrypt(self,msg:str) -> str:
        return (base64.b64encode((PKCS1_OAEP.new(self.__keys[0]).encrypt(msg.encode())))).decode("ascii")

    def decrypt(self,msg:str) -> str :
        return (PKCS1_OAEP.new(self.__keys[1])).decrypt(base64.b64decode(msg.encode("ascii"))).decode()

        

if __name__=="__main__":
    client_rsa=RSA_Class()
    server_rsa=RSA_Class()

    msg="גולן המלך אין עליו"[::-1]
    msg="34.76.80.113;;;3213;;;Golan is the best teacher I have met,he will give me 100 behazrat ha shem"+"a"*210
    msg=';;1.1.1.1;80;;;dddddddddddddddddddddddddddddddddddddddddddddddddddddddddddaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccceeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeedddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddd'
    msg="1.1.1.1;;;80;;;hello"
    msg="\r\r\r\r\r\r\r\r\r\r\r\r\r\r\n\n\n\n\n\n\n\n\n\n\n\n"

    client_rsa.get_msg_in_parts(msg)

    # length=len(msg.encode())
    # print(length)
    # client_rsa.modulus_length=length,False

    # print(str(client_rsa.modulus_length))

    # print("c: givving")
    # print("s: ready")
    # server_rsa.modulus_length=length,False


