#coding:utf-8
#----------------------------------------------------httpd.py 1.0-------------------------------------------------------
#作者：曾辰
#日期：2016-08-04
#
#   httpd.py一个html服务程序的开源项目，它最初构建的目的是作者为了清楚了解html服务程序的运行机制，本代码参照了一开源项目
#tinyhttpd（C语言）。
#   因为此版本是由python语言编写，故取名为httpd.py。这个项目力求以最简短的代码完成一个简陋的服务器软件，最终整个项目以100
#行左右的代码实现了一个静态html服务器程序。
#   如果你希望弄懂html服务器软件的原理而又不希望深入了解，或者是不愿意花大把时间去读令人头疼的apache代码，那么本项目将是
#你最佳的练手选择。你只需花上两个小时的时间，仔细研读这份python代码，这将会使你对http工作机制有更深入的了解。
#
#-----------------------------------------------------------------------------------------------------------------------
import socket
import re
import thread

def main():
    '''
    主函数：整个程序的入口
    '''
    server=startup()
    if server:
        #循环等待接收请求
        while True:
            conn,address=server.accept()
            if conn:
                buf=conn.recv(1024)
                if buf:
                    #判断GET和POST请求是否在其中，如不在其中，则返回错误请求码
                    if 'GET' in buf or 'POST' in buf:
                        #开启新线程处理请求
                        thread.start_new_thread(accept_request,(buf,conn))
                    else:
                        bad_request(con)
            else:
                pass

def startup():
    '''
        初始化：负责端口的绑定及监听
    '''
    host=('localhost',8081)
    try:
        server=socket.socket(socket.AF_INET,socket.SOCK_STREAM)
        server.bind(host)
        server.listen(50)
    except Exception,error:
        print error
    return server

def accept_request(buf,conn):
    '''
    请求处理：负责处理客户端访问请求
    '''
    req=buf.split('\r\n')
    for e in req:
        e=e.split(' ')
    #处理GET或者POST信息，获取请求路径
    url=req[0].split(' ')[1]
    #判断请求路径是否为目录，如果是目录则默认加上index.html
    if url.endswith('/'):
        url='www'+url+'index.html'
    else:
        url='www'+url
    try:
        with open(url,'rb') as f:
            content=f.read()
            headers(conn)
            conn.send(content)
    except:
        #没有找到资源
        not_found(conn)
    finally:
        conn.close()

def bad_request(client_conn):
    '''
    错误访问：返回错误访问代码
    '''
    client_conn.send("HTTP/1.0 BAD REQUEST\r\n")
    client_conn.send("Content-Type:text/html\r\n")
    client_conn.send("\r\n")
    client_conn.send("<p>Your browser sent a bad request,such as a POST without a Content-Lenght.</p>")
    
def headers(client_conn):
    '''
    消息头：返回协议头
    '''
    client_conn.send('HTTP/1.0 200 OK\r\n')
    client_conn.send('Server:httpd.py 1.0\r\n')
    client_conn.send('Content-Type:text/html\r\n')
    client_conn.send('\r\n')
    
def not_found(client_conn):
    '''
    资源没有找到：返回资源没有找到到的错误信息
    '''
    client_conn.send("HTTP/1.0 404 not found\r\n")
    client_conn.send("Content-Type:text/html\r\n")
    client_conn.send("\r\n")
    client_conn.send("<h1 text_align='center'>404 not found</h1>")
    
main()
