# -*- coding: utf-8 -*-
# -*- coding: cp949 -*-
import sys
from PyQt5.QtCore import QModelIndex
from PyQt5.QtWidgets import *
from PyQt5.QAxContainer import *
import threading
import thread
import time
import os
import numpy as np
import pandas as pd
import pymssql
Stop_Auto_Trading = True
_scrNum = 5000
con = pymssql.connect(host='127.0.0.1:21433', user='sa', password='sj12sj12', database='StockInformation1')
lock = thread.allocate_lock()
gthreshold=3
row_index=5
def GetSrcNum():
    global _scrNum
    global lock
    if _scrNum < 9999:
        lock.acquire()
        _scrNum+=1
        lock.release()
    else:
        _scrNum = 5000;
    return _scrNum;

def determinePosition(df, column, curPrice):
    current_price = curPrice
    global row_index
    df_moving_average = df[column].rolling(window=row_index).mean()
    df_moving_average_std = df[column].rolling(window=row_index).std()

    moving_average = df_moving_average[row_index]
    moving_average_std = df_moving_average_std[row_index]

    price_arbitrage = current_price - moving_average
    global gthreshold
    threshold = gthreshold
    if abs(price_arbitrage) > moving_average_std * threshold:
        if np.sign(price_arbitrage) > 0:
            return 1 #매도
        else:
            return 2 #매수
    return 3 #hold

class Select_Algorithm_Dlg(QDialog):
    def __init__(self):
        QDialog.__init__(self)
        self.setWindowTitle("Select Algorithm")
        self.setGeometry(250,100,410,500)
        self.setFixedSize(410,500)

        self.combo = QComboBox(self)
        self.combo.addItem("=========Select Algorithm=========")
        self.combo.addItem("표준편차 알고리즘")
        self.combo.addItem("급등주 알고리즘")

        self.result_Text = QTextEdit(self)

        self.lb = QLabel("기준 추세선: ", self)
        self.aver_le = QLineEdit(self)
        self.lb2 = QLabel("일, ", self)
        self.lb3 = QLabel("표준편차 배수",self)
        self.aver_std_le = QLineEdit(self)
        self.lb4 = QLabel("배", self)
        self.apply_btn = QPushButton("적용",self)

        self.apply_btn.clicked.connect(self.apply_Btn_Click)

        self.combo.setGeometry(20,20,300,20)
        self.result_Text.setGeometry(20,85,350,400)

        self.lb.setGeometry(30, 40, 70, 20)
        self.aver_le.setGeometry(100,40,35,20)
        self.lb2.setGeometry(140, 40, 50, 20)
        self.lb3.setGeometry(160,40,80,20)
        self.aver_std_le.setGeometry(245,40,35,20)
        self.lb4.setGeometry(290,40,50,20)
        self.apply_btn.setGeometry(160,65,50,20)

        self.result_Text.setReadOnly(False)
        self.lb.setVisible(False)
        self.aver_le.setVisible(False)
        self.lb2.setVisible(False)
        self.lb3.setVisible(False)
        self.aver_std_le.setVisible(False)
        self.lb4.setVisible(False)
        self.apply_btn.setVisible(False)

        self.lb.setVisible(False)
        self.combo.currentIndexChanged.connect(self.On_Select)

    def apply_Btn_Click(self):
        global row_index,gthreshold
        row_index = int(self.aver_le.text())
        gthreshold = int(self.aver_std_le.text())
    def On_Select(self):
        result = self.combo.currentIndex()
        if result==1:
            self.result_Text.setText("기준 추세선이란 이동평균선을 이야기 하는 것으로 5일 20일 60일 120일의 평균주가를 나타낸 것으로서 "
                                     "매수매도를 진행할때 기준선이된다.                                                                     "
                                     "표준편차 배수란 주가에 변동이있을때 기준선과 주가의 차이를 구해 차이가 표준편차의 배수를 넘었을때 "
                                     "매수매도를 진행하므로 수익률에 막대한 영향을 미치는 중요한 요소이다. "
                                     "배수가 너무 낮으면 큰이익 이 없으며 너무 높다면 매수매도시기를 놓칠수 있다.")
            self.lb.setVisible(True)
            self.aver_le.setVisible(True)
            self.lb2.setVisible(True)
            self.lb3.setVisible(True)
            self.aver_std_le.setVisible(True)
            self.lb4.setVisible(True)
            self.apply_btn.setVisible(True)
        elif result==2:
            self.result_Text.setText("급등주 알고리즘")
            self.lb.setVisible(False)
            self.aver_le.setVisible(False)
            self.lb2.setVisible(False)
            self.lb3.setVisible(False)
            self.aver_std_le.setVisible(False)
            self.lb4.setVisible(False)
            self.apply_btn.setVisible(False)
        else:
            self.result_Text.setText("")
            self.lb.setVisible(False)
            self.aver_le.setVisible(False)
            self.lb2.setVisible(False)
            self.lb3.setVisible(False)
            self.aver_std_le.setVisible(False)
            self.lb4.setVisible(False)
            self.apply_btn.setVisible(False)

class Insert_sCode_Dlg(QDialog):
    def __init__(self):
        QDialog.__init__(self)
        self.setWindowTitle("Insert Auto-Trading Stock Code")
        self.setGeometry(250,100,410,500)
        self.setFixedSize(410,500)

        self.sCodeEdit = QLineEdit(self)
        search = QPushButton("조회",self)
        insert = QPushButton("등록",self)
        delete = QPushButton("삭제",self)
        self.sCodeList = QListWidget(self)
        sCodelb = QLabel("종목코드:",self)
        self.sNamelb = QLabel("종목명:", self)
        self.lbPER = QLabel("PER:",self)
        self.lbEPS = QLabel("EPS:", self)
        self.lbROE = QLabel("ROE:", self)
        self.lbPBR = QLabel("PBR:", self)
        self.lbBPS = QLabel("BPS:", self)
        self.lbEV = QLabel("EV:", self)

        sCodelb.setGeometry(45, 30, 80, 30)
        self.sCodeEdit.setGeometry(115, 30, 100, 30)
        search.setGeometry(225,30,50,30)
        insert.setGeometry(30,110, 50, 30)
        delete.setGeometry(240, 110, 50, 30)
        self.sCodeList.setGeometry(20,145,280,340)
        self.sNamelb.setGeometry(305,120,100,30)
        self.lbPER.setGeometry(305,150,100,30)
        self.lbEPS.setGeometry(305,180,100,30)
        self.lbROE.setGeometry(305,210,100,30)
        self.lbPBR.setGeometry(305,240,100,30)
        self.lbBPS.setGeometry(305,270,100,30)
        self.lbEV.setGeometry(305,300,100,30)

        self.lbPER.setToolTip("PER (주가수익비율) : 배당금에 따른 이익")
        self.lbEPS.setToolTip("EPS (주당순이익) : 수익에 따른 주주의 몫")
        self.lbROE.setToolTip("ROE (자기자본이익률) : 이익창출능력판단 ")
        self.lbPBR.setToolTip("PBR (주가순자산비율) : 기업의 순자산가치를 판단")
        self.lbBPS.setToolTip("BPS (주당순자산) : 부채 청산후 남는 가치 판단")
        self.lbEV.setToolTip("EV (기업가치) : 기업매수자 매수시 지급해야 하는 금액")




        self.updateitem()

        search.clicked.connect(self.On_Search_Clicked)
        insert.clicked.connect(self.On_Insert_Clicked)
        delete.clicked.connect(self.On_Delete_Clicked)

    def updateitem(self):
        self.sCodeList.clear()
        for item in til.keys():
            s = "{}".format(item)
            self.sCodeList.addItem(s)

    def On_Search_Clicked(self):
        sCode = self.sCodeEdit.text()
        df = pd.read_sql("select * from Stock_Type_Name where 종목코드 in('{}')".format(sCode), con)
        try:
            s=10
            s = (df.icol(1).irow(0)).encode('utf8')
            self.sNamelb.setText("종목명: {}".format(s))
            self.lbPER.setText("PER: {}".format(df.icol(3).irow(0)))
            self.lbEPS.setText("EPS: {}".format(df.icol(4).irow(0)))
            self.lbROE.setText("ROE: {}".format(df.icol(5).irow(0)))
            self.lbPBR.setText("PBR: {}".format(df.icol(6).irow(0)))
            self.lbBPS.setText("BPS: {}".format(df.icol(7).irow(0)))
            self.lbEV.setText("EV: {}".format(df.icol(8).irow(0)))
        except:
            QMessageBox.about(self,'Stock Error',"종목코드가 없습니다.")
            self.sNamelb.setText("종목명:")
            self.lbPER.setText("PER:")
            self.lbEPS.setText("EPS:")
            self.lbROE.setText("ROE:")
            self.lbPBR.setText("PBR:")
            self.lbBPS.setText("BPS:")
            self.lbEV.setText("EV:")

    def On_Insert_Clicked(self):
        sCode = self.sCodeEdit.text()
        if sCode in til.keys():
            QMessageBox.about(self, 'Stock Error', "이미 등록된 코드입니다.")
        else:
            til.addList(sCode,0,threading.Event())
        self.updateitem()

    def On_Delete_Clicked(self):
        sCode = self.sCodeEdit.text()
        try:
            curitem = self.sCodeList.currentItem()
            sCode = curitem.text()
            til.pop(sCode)
            self.updateitem()
        except:
            QMessageBox.about(self, 'Choose Error', "선택된 항목이 없습니다.")


class TradinginfoList(dict):
    def addList(self,Code,price,event,mount=0):
        self[Code] = [price,event,mount]

til = TradinginfoList()

class BS_Thread(threading.Thread):
    def __init__(self, lb,api,stockCode):
        threading.Thread.__init__(self)
        self.list = lb
        self.kiwoom = api
        self.Code = stockCode
        global con
        self.df = pd.read_sql("Select * from Day_C_Data3 where 종목코드 in('{}') order by 일자 desc".format(self.Code), con)
        self.screenNo = GetSrcNum()
        self.kiwoom.dynamicCall("SetRealReg(QString,QString,QString,QString)",self.screenNo,self.Code,"10","1")

    def run(self):
        while True:
            if self.df.icol(3).irow(0) < 3000:
                self.kiwoom.dynamicCall("DisconnectRealData(QString)",self.screenNo)
                break
            AccNO = self.kiwoom.dynamicCall("GetLoginInfo(QString)", ["ACCNO"])

            til[self.Code][1].wait()

            global Stop_Auto_Trading
            if Stop_Auto_Trading:
                break
            curprice = til[self.Code][0]

            position = determinePosition(self.df , 'Close' , int(curprice))
            #현재가 넣어준다
            #50만원내에서 주문수량결정
            mount = int(500000 / int(curprice))

            if position == 1:
                if til[self.Code][2] > 0:
                    smount = til[self.Code][2]
                    self.kiwoom.dynamicCall("SendOrder(QString, QString, QString, int, QString, int, int, QString, QString)",
                                            ["주식주문", GetSrcNum(), AccNO[:10], 2, str(self.Code), smount, 0, "03", ""])
                    self.list.addItem("Sell : {}  {}  {}".format(self.Code,curprice,smount))
                    til[self.Code][2] = 0
            elif position == 2:
                if til[self.Code][2] == 0:
                    self.kiwoom.dynamicCall(
                        "SendOrder(QString, QString, QString, int, QString, int, int, QString, QString)",
                        ["주식주문", GetSrcNum(), AccNO[:10], 1, str(self.Code), mount, 0, "03", ""])
                    self.list.addItem("Buy : {}  {}  {}".format(self.Code, curprice, mount))
                    til[self.Code][2] = mount
                #사용자구분명,화면번호,계좌번호,주문유형,종목코드,주문수량,주문가격,거래구분,원주문번호
            til[self.Code][1].clear()
thlist = {}
class Trading_Model():
    def __init__(self,kiwoom,lb):
        self.api = kiwoom
        self.list = lb
        self.t = time
    def connect(self):
        state = self.api.dynamicCall("GetConnectState()")
        if state==0:
            self.api.dynamicCall("CommConnect()")
    def disconnect(self):
        state = self.api.dynamicCall("GetConnectState()")
        if state==1:
            self.api.dynamicCall("CommTerminate()")
            self.disconnectRealData()
            self.list.addItem("로그아웃 되었습니다.")

    def disconnectRealData(self):
        for i in range(5000,9999):
            self.api.dynamicCall("DisconnectRealData(QString)",i)


#######################################################################################
#######################################################################################
    def bs_model(self):
        for i in til.keys():
            th1 = BS_Thread(self.list, self.api, i)
            thlist.items().append(th1)
            th1.start()
            threading._sleep(0.2)

#######################################################################################
#######################################################################################
class MainWindow(QMainWindow):
    def __init__(self):
        QMainWindow.__init__(self)
        self.setWindowTitle("Stocker's")
        self.setGeometry(250,100,1000,600)
        self.kiwoom = QAxWidget("KHOPENAPI.KHOpenAPICtrl.1")
        self.kiwoom.OnReceiveRealData.connect(self.OnReceiveRealData)
        self.kiwoom.OnEventConnect.connect(self.OnEventConnect)

        self.lb = QListWidget(self)
        self.lb.setGeometry(40, 40, 500, 500)
        self.tm = Trading_Model(self.kiwoom,self.lb)

        # 버튼 생성
        Start_Auto_Btn = QPushButton("Start-Auto-Trading", self)
        Start_Auto_Btn.setGeometry(600,100,200,50)
        Start_Auto_Btn.clicked.connect(self.Auto_Trading_Btn_Click)

        Stop_Auto_Btn = QPushButton("Stop-Auto-Trading", self)
        Stop_Auto_Btn.setGeometry(600,200,200,50)
        Stop_Auto_Btn.clicked.connect(self.Stop_Btn_Click)

        bar = self.menuBar()
        bar.setNativeMenuBar(True)

        file = bar.addMenu('파일')
        search = bar.addMenu('조회')
        chart = bar.addMenu('차트')
        setting = bar.addMenu('설정')
        help = bar.addMenu('도움말')

        login = file.addAction('로그인')
        logout = file.addAction('로그아웃')

        login.triggered.connect(self.tm.connect)
        logout.triggered.connect(self.tm.disconnect)

        chart_Maping = chart.addAction('차트매칭')
        chart_Maping.triggered.connect(self.ChartMaping_Clicked)


        set_auto_trading = setting.addMenu('자동매매 설정')
        seaerh_Insertmi = set_auto_trading.addAction('관심종목 등록 및 조회')
        seaerh_Insertmi.triggered.connect(self.Insertmi_Clicked)

        set_algorithm = set_auto_trading.addAction('자동매매 알고리즘 설정')
        set_algorithm.triggered.connect(self.Set_Algorithm_Clicked)
        self.tm.connect()
        self.kiwoom.dynamicCall("CommConnect()")
    def __del__(self):
        global Stop_Auto_Trading
        Stop_Auto_Trading = True
    def ChartMaping_Clicked(self):
        os.startfile("C:\Matching_Algorithm.exe")
    def Insertmi_Clicked(self):
        a = Insert_sCode_Dlg()
        a.show()
        a.exec_()
    def Set_Algorithm_Clicked(self):
        a = Select_Algorithm_Dlg()
        a.show()
        a.exec_()

    def OnEventConnect(self,errCode):
        if errCode==0:
            self.lb.addItem("로그인 성공")
        else:
            self.lb.addItem("로그인 실패 다시 로그인 해주세요")
######실시간 데이터 처리#######
#################################################################
#################################################################
#################################################################
    def OnReceiveRealData(self,sCode,sRealType,sRealData):
        type = sRealType.encode('utf-8')
        if type == "주식체결":
            curprice = self.kiwoom.dynamicCall("GetCommRealData(QString,int)",sCode,10)
            til[sCode][0] = curprice[1:]
            til[sCode][1].set()
#################################################################
#################################################################
#################################################################
    def Auto_Trading_Btn_Click(self,event):
        global Stop_Auto_Trading
        if Stop_Auto_Trading==True:
            Stop_Auto_Trading = False
            self.tm.bs_model()
        else:
            QMessageBox.about(self, 'Trading Error', "자동매매가 진행중입니다.")
    def Stop_Btn_Click(self,event):
        global Stop_Auto_Trading
        Stop_Auto_Trading = True

app = QApplication(sys.argv)
Chart = MainWindow()
Chart.show()
sys.exit(app.exec_())