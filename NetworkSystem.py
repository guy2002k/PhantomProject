import urllib.request as request
import netifaces
import ipaddress

class Network_Properties():

    @staticmethod
    def external_ip() -> str:
        return request.urlopen('https://ident.me').read().decode('ascii')


    @staticmethod
    def local_ip() -> str:
        gateway=netifaces.gateways()['default'][netifaces.AF_INET][0]

        for i in netifaces.interfaces():
            try:
                ip=netifaces.ifaddresses(i)[netifaces.AF_INET][0]['addr']
                mask=netifaces.ifaddresses(i)[netifaces.AF_INET][0]['netmask']

                if(ipaddress.IPv4Address(ip) in ipaddress.ip_network(str(gateway+"/"+mask),strict=False)):
                    return ip

            except: pass      


if __name__=="__main__":
    print(Network_Properties.local_ip())     