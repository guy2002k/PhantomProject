from Packet import Packet


class Tetris_Tor_Encryption():
      
      @staticmethod
      def encrypt(packet:Packet) -> str:
        return packet.get(Packet.Options.IP)+";;;"+str(packet.get(Packet.Options.PORT))+";;;"+packet.get(Packet.Options.REQUEST)

      @staticmethod
      def decrypt(msg:str) -> Packet:
        a=msg.split(";;;")
        return Packet(a[0],int(a[1]),a[2])

