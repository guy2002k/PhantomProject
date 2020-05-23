from enum import Enum,auto

'''
This clas is for storing packets object
'''

class Packet():

    class Options(Enum):
        IP=auto(),
        PORT=auto(),
        HOSTNAME=auto(),
        REQUEST=auto(),
        ALL=auto()

    def __init__(self,ip:str,port:int,request="",hostname=""):
        if(self.check(ip,Packet.Options.IP)):
           self.__ip=ip
        else:
            self.__error_raiser(Exception("Your ip is not right, Please check him!"))


        if(self.check(port,Packet.Options.PORT)):   
          self.__port=port
        else:
            self.__error_raiser(Exception("Your port is not right, Please check him!"))

        self.__request=request
        self.__hostname=hostname

    def __error_raiser(self,error:Exception): raise error

    def get(self,option:Options):
      if(option is Packet.Options.IP):
          return self.__ip 
      elif(option is Packet.Options.PORT):
          return self.__port
      elif(option is Packet.Options.HOSTNAME):
          return self.__hostname     
      elif(option is Packet.Options.REQUEST):
          return self.__request    
      else:
          return (self.__ip,self.__port,self.__request,self.__hostname)


    def set(self,value,option:Options):
      if(option is Packet.Options.IP):
          self.__ip=value if(type(value) is str) else self.__error_raiser(Exception("Your ip is not right, Please check him!"))
      elif(option is Packet.Options.PORT):
          self.__port=value if(type(value) is int) else self.__error_raiser(Exception("Your port is not right, Please check him!"))
      elif(option is Packet.Options.REQUEST):
          self.__request=value if(type(value) is str) else self.__error_raiser(Exception("Your request is not right, Please check him!"))         

    def check(self,obj,option:Options):
      if(option is Packet.Options.IP):
          if(obj.count(".")==3):
              adress=obj.split(".")
              for num in adress:
                  num=int(num)
                  if(num<0 and num>255):
                      return False
              return True

      elif(option is Packet.Options.PORT):
          if(obj>1 and obj<65535):
              return True
      
      return False                                               