import firebase_admin
import json
import os
import psutil
import pathlib


from firebase_admin import credentials
from firebase_admin import firestore

class Check_Server:

    @staticmethod
    def string_cutter(path:str):
        path=path.split("\\")

        for i in range(0):
            path.pop(len(path)-1)

        return '\\'.join(path)    


    @staticmethod            
    def is_server_alive():
        path=Check_Server.string_cutter(str(pathlib.Path().absolute()).replace(r'\\',r'\\\\'))

        server_prop_path=path+"\\Jsons\\ServersProp.json"

        tor_certificate_path=path+"\\Jsons\\Tor-Certificate.json"

        while(True):
            with open(server_prop_path) as server_prop_file:
                properties=json.load(server_prop_file)
                proc_id=properties[0]["Pid"]
                server_name=properties[0]["Name"]


            if(proc_id!=-1 and server_name!=""):
                if(not(psutil.pid_exists(proc_id))):
                    data=[]

                    firebase_admin.initialize_app(credentials.Certificate(tor_certificate_path))
                    db=firestore.client()

                    db.collection("Servers").document(server_name).delete()

                    data.append({
                                "Pid":-1,
                                "Name":""
                                })

                    with open(server_prop_path ,'w') as server_prop_file:
                            json.dump(data,server_prop_file)


                    (psutil.Process(os.getpid())).terminate()       




if __name__=="__main__":
    Check_Server.is_server_alive()
