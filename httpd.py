# --* coding: utf-8 *--
import socket
import re
import _thread

# global parameters 
paras = {
    "host": "localhost",
    "port": 1314,
    "relative_path": "www"
    }

def init():
    '''
        initialize web server, include process port and listen
    '''
    host = (paras["host"],paras["port"])
    server = socket.socket(socket.AF_INET,socket.SOCK_STREAM)
    server.bind(host)
    server.listen(50)
    return server

def accept_request(buf, con):
    '''
        handle request from user host
    '''
    req = buf.split('\r\n')
    for e in req:
        e = e.split(' ')
    url = req[0].split(' ')[1]
    print(url, end = "\n")
    if url.endswith('/'):
        url = paras["relative_path"] + url + "index.html"
    else:
        url = paras["relative_path"] + "/" + url
    try:
        with open(url,"rb") as f:
            content = f.read()
            headers(con)
            con.send(content)
    except:
        not_found(con)
    finally:
        con.close()

def bad_request(client_con):
    '''
        return error code
    '''
    client_con.send(bytes("HTTP/1.0 BAD REQUEST\r\n", "utf-8"))
    client_con.send(bytes("Content-Type:text/html\r\n", "utf-8"))
    client_con.send(bytes("\r\n", "utf-8"))
    client_con.send(bytes("<p>Your browser sent a bad request,such as a POST without a Content-Lenght.</p>", "utf-8"))
    
def headers(client_con):
    '''
        return header
    '''
    client_con.send(bytes("HTTP/1.0 200 OK\r\n", "utf-8"))
    client_con.send(bytes("Server:httpd.py 1.0\r\n", "utf-8"))
    client_con.send(bytes("Content-Type:text/html\r\n", "utf-8"))
    client_con.send(bytes("\r\n", "utf-8"))
    
def not_found(client_con):
    '''
        not found
    '''
    client_con.send(bytes("HTTP/1.0 404 not found\r\n", "utf-8"))
    client_con.send(bytes("Content-Type:text/html\r\n", "utf-8"))
    client_con.send(bytes("\r\n", "utf-8"))
    client_con.send(bytes("<h1 text_align='center'>404 not found</h1>", "utf-8"))
if __name__ == "__main__":
    server = init()
    if server:
        while True:
            con, address = server.accept()
            print("host:", address, end = "")
            if con:
                buf = str(con.recv(1024))
                if buf:
                    if "GET" in buf or "POST" in buf:
                        _thread.start_new_thread(accept_request,(buf, con))
                    else:
                        bad_request(con)
            else:
                pass
    else:
        print("Error: server can not start.You may check the system port occupancy")
    
