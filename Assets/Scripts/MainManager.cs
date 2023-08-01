using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using SimpleJSON;
using SocketIO;
using System.Net.Sockets;
using System.Net;

public class MainManager : MonoBehaviour
{
    public InputField ipField;
    public GameObject socketPrefab;
    public GameObject popup;
    public Text msg;
    GameObject socketObj;
    SocketIOComponent socket;
    private bool is_socket_open = false;
    public AudioSource soundObj;

    void Start()
    {
    }

    void InitSocketFunctions()
    {
        socketObj = Instantiate(socketPrefab);
        socket = socketObj.GetComponent<SocketIOComponent>();
        socket.On("open", socketOpen);
        socket.On("error", socketError);
        socket.On("close", socketClose);
    }

    public void socketOpen(SocketIOEvent e)
    {
        if (is_socket_open)
        {
            return;
        }
        if (socket != null)
        {
            is_socket_open = true;
            socket.Emit("testSocketSetInfo");
            Debug.Log("[SocketIO] Open received: " + e.name + " " + e.data);
            msg.text = "socket이 연결되었습니다.";
            popup.SetActive(true);
        }
    }

    public void socketError(SocketIOEvent e)
    {
        Debug.Log("[SocketIO] Error received: " + e.name + " " + e.data);
        msg.text = "socket접속에서 오류가 발생했습니다.";
        popup.SetActive(true);
    }

    public void socketClose(SocketIOEvent e)
    {
        Debug.Log("[SocketIO] Close received: " + e.name + " " + e.data);
        is_socket_open = false;
        msg.text = "socket접속이 끊어졌습니다.";
        popup.SetActive(true);
    }

    public void SaveIp()
    {
        if(ipField.text == "")
        {
            msg.text = "ip를 입력하세요.";
            popup.SetActive(true);
        }
        else
        {
            StartCoroutine(saveIpProcess());
        }
    }

    IEnumerator saveIpProcess()
    {
        if (socket != null)
        {
            socket.Close();
            socket.OnDestroy();
            socket.OnApplicationQuit();
        }
        if (socketObj != null)
        {
            DestroyImmediate(socketObj);
        }

        yield return new WaitForSeconds(1f);
        Global.socket_server = "ws://" + ipField.text + ":3006";
        InitSocketFunctions();
        msg.text = "저장되었습니다!";
        popup.SetActive(true);
    }

    public void flowmeter_start()
    {
        if (socket != null)
        {
            string data = "{\"id\":2}";
            Debug.Log("flowmeter_start event." + data);
            socket.Emit("flowmeterStart", JSONObject.Create(data));
            msg.text = "이벤트 발생!";
            popup.SetActive(true);
        }
        else
        {
            msg.text = "소켓을 연결하세요!";
            popup.SetActive(true);
        }
    }

    public void flowmeter_value50()
    {
        if (socket != null)
        {
            string data = "{\"id\":2,\"value\":50}";
            Debug.Log("flowmeter_value50 event." + data);
            socket.Emit("flowmeterValue", JSONObject.Create(data));
            msg.text = "이벤트 발생!";
            popup.SetActive(true);
        }
        else
        {
            msg.text = "소켓을 연결하세요!";
            popup.SetActive(true);
        }
    }

    public void flowmeter_value100()
    {
        if (socket != null)
        {
            string data = "{\"id\":2,\"value\":100}";
            Debug.Log("flowmeter_value100 event." + data);
            socket.Emit("flowmeterValue", JSONObject.Create(data));
            msg.text = "이벤트 발생!";
            popup.SetActive(true);
        }
        else
        {
            msg.text = "소켓을 연결하세요!";
            popup.SetActive(true);
        }
    }

    public void flowmeter_finish200()
    {
        if (socket != null)
        {
            string data = "{\"id\":2,\"type\":0,\"value\":250,\"is_pay_after\":1}";
            Debug.Log("flowmeter_finish_200 event." + data);
            socket.Emit("flowmeterFinish", JSONObject.Create(data));
            msg.text = "이벤트 발생!";
            popup.SetActive(true);
        }
        else
        {
            msg.text = "소켓을 연결하세요!";
            popup.SetActive(true);
        }
    }

    public void flowmeter_finish200Prepaid()
    {
        if (socket != null)
        {
            string data = "{\"id\":2,\"type\":0,\"value\":250,\"is_pay_after\":0,\"remain_value\":30}";
            Debug.Log("flowmeter_finish_200_prepaid event." + data);
            socket.Emit("flowmeterFinish", JSONObject.Create(data));
            msg.text = "이벤트 발생!";
            popup.SetActive(true);
        }
        else
        {
            msg.text = "소켓을 연결하세요!";
            popup.SetActive(true);
        }
    }

    public void flowmeter_finish_remain()
    {
        if (socket != null)
        {
            string data = "{\"id\":2,\"type\":2,\"value\":250,\"remain_value\":30}";
            Debug.Log("flowmeter_finish_remain event." + data);
            socket.Emit("flowmeterFinish", JSONObject.Create(data));
            msg.text = "이벤트 발생!";
            popup.SetActive(true);
        }
        else
        {
            msg.text = "소켓을 연결하세요!";
            popup.SetActive(true);
        }
    }

    public void flowmeter_finish_soldout()
    {
        if (socket != null)
        {
            string data = "{\"id\":2,\"type\":1,\"value\":250}";
            Debug.Log("flowmeter finish soldout event." + data);
            socket.Emit("flowmeterFinish", JSONObject.Create(data));
            msg.text = "이벤트 발생!";
            popup.SetActive(true);
        }
        else
        {
            msg.text = "소켓을 연결하세요!";
            popup.SetActive(true);
        }
    }

    public void shopOpen()
    {
        if(socket != null)
        {
            Debug.Log("shopOpenEvent!");
            socket.Emit("shopOpen");
            msg.text = "이벤트 발생!";
            popup.SetActive(true);
        }
        else
        {
            msg.text = "소켓을 연결하세요!";
            popup.SetActive(true);
        }
    }

    public void shopClose()
    {
        if (socket != null)
        {
            string sId = "{\"id\":2}";
            Debug.Log("shopclose event." + sId);
            socket.Emit("shopClose", JSONObject.Create(sId));
            msg.text = "이벤트 발생!";
            popup.SetActive(true);
        }
        else
        {
            msg.text = "소켓을 연결하세요!";
            popup.SetActive(true);
        }
    }

    public void tagLock()
    {
        if (socket != null)
        {
            string tagGWData = "{\"tagGW_no\":1,\"ch_value\":2,\"status\":0\"}";
            Debug.Log("tag lock event." + tagGWData);
            socket.Emit("deviceTagLock", JSONObject.Create(tagGWData));
            msg.text = "이벤트 발생!";
            popup.SetActive(true);
        }
        else
        {
            msg.text = "소켓을 연결하세요!";
            popup.SetActive(true);
        }
    }

    public void tagRelease()
    {
        if (socket != null)
        {
            string tagGWData = "{\"tagGW_no\":1,\"ch_value\":2,\"status\":1\"}";
            Debug.Log("tag release event." + tagGWData);
            socket.Emit("deviceTagLock", JSONObject.Create(tagGWData));
            msg.text = "이벤트 발생!";
            popup.SetActive(true);
        }
        else
        {
            msg.text = "소켓을 연결하세요!";
            popup.SetActive(true);
        }
    }

    public void valveAopen()
    {
        if (socket != null)
        {
            string tagGWData = "{\"tagGW_no\":1,\"ch_value\":2,\"status\":1\"}";
            Debug.Log("valve a open event." + tagGWData);
            socket.Emit("deviceTagLock", JSONObject.Create(tagGWData));
            msg.text = "이벤트 발생!";
            popup.SetActive(true);
        }
        else
        {
            msg.text = "소켓을 연결하세요!";
            popup.SetActive(true);
        }
    }

    public void valveAclose()
    {
        if (socket != null)
        {
            string data = "{\"board_no\":1," + "\"ch_value\":2," + "\"valve\":0," + "\"status\":1\"}";
            Debug.Log("valve a close event." + data);
            socket.Emit("boardValveCtrl", JSONObject.Create(data));
            msg.text = "이벤트 발생!";
            popup.SetActive(true);
        }
        else
        {
            msg.text = "소켓을 연결하세요!";
            popup.SetActive(true);
        }
    }

    public void soldout()
    {
        if (socket != null)
        {
            string data = "{\"id\":2}";
            Debug.Log("soldout event." + data);
            socket.Emit("soldout", JSONObject.Create(data));
            msg.text = "이벤트 발생!";
            popup.SetActive(true);
        }
        else
        {
            msg.text = "소켓을 연결하세요!";
            popup.SetActive(true);
        }
    }

    public void popupEvent1()
    {
        if (socket != null)
        {
            string data = "{\"id\":2," + "\"type\":0," + "\"is_close\":0," + "\"content\":";
            string content = @"서버와 접속 중입니다.\nConnecting to POS";
            data += "\"" + content + "\"}";
            Debug.Log("popup1 event." + data);
            socket.Emit("errorReceived", JSONObject.Create(data));
            msg.text = "이벤트 발생!";
            popup.SetActive(true);
        }
        else
        {
            msg.text = "소켓을 연결하세요!";
            popup.SetActive(true);
        }
    }

    public void popupEvent2()
    {
        if (socket != null)
        {
            string data = "{\"id\":2," + "\"type\":1," + "\"content\":";
            string content = @"서버와 접속 중입니다.\nConnecting to POS";
            data += "\"" + content + "\"}";
            Debug.Log("popup2 event." + data);
            socket.Emit("errorReceived", JSONObject.Create(data));
            msg.text = "이벤트 발생!";
            popup.SetActive(true);
        }
        else
        {
            msg.text = "소켓을 연결하세요!";
            popup.SetActive(true);
        }
    }

    public void adminEvent()
    {
        if (socket != null)
        {
            string data = "{\"id\":2}";
            Debug.Log("admin received event:" + data);
            socket.Emit("adminReceived", JSONObject.Create(data));
            msg.text = "이벤트 발생!";
            popup.SetActive(true);
        }
        else
        {
            msg.text = "소켓을 연결하세요!";
            popup.SetActive(true);
        }
    }

    public void infoChanged()
    {
        if (socket != null)
        {
            string data = "{\"id\":2,\"server_id\":100,\"gw_no\":1,\"gw_channel\":2,"
                + "\"board_no\":1,\"board_channel\":2,\"decarbo_time\":10,\"opentime\":60,"
                + "\"soldout\":500,"
                + "\"is_soldout\":0,"
                //+ "\"bintage\":2020,"
                + "\"country\":\"미국/캘리포니아/나파밸리\","
                + "\"description\":";
            string content = @"블라인드테스트 1위 와인 \n 나파밸리를 대표하는 와인입니다.";
            data += "\"" + content + "\","
                 + "\"drink_name\":";
            string drink_name = @"나파밸리 카베르네 소비뇽";
            data += "\"" + drink_name + "\","
                //+ "\"kind\":\"레드\","
                + "\"serial_number\":2,"
                + "\"app_type\":0,"
                + "\"appNo\":2,"
                + "\"product_id\":2,"
                + "\"product_name\":2,"
                + "\"cup_size\":200,"
                + "\"sell_type\":0,"
                + "\"unit_price\":2,"
                + "\"styles\":\"카베르네 소비뇽, 메를로\"}";
            Debug.Log("Info Changed event." + JSONObject.Create(data));
            socket.Emit("testInfoChanged", JSONObject.Create(data));
            msg.text = "이벤트 발생!";
            popup.SetActive(true);
        }
        else
        {
            msg.text = "소켓을 연결하세요!";
            popup.SetActive(true);
        }
    }

    public void reparingDevice()
    {
        if (socket != null)
        {
            string data = "{\"id\":2}";
            Debug.Log("ReparingDevice event." + data);
            socket.Emit("RepairingDevice", JSONObject.Create(data));
            msg.text = "이벤트 발생!";
            popup.SetActive(true);
        }
        else
        {
            msg.text = "소켓을 연결하세요!";
            popup.SetActive(true);
        }
    }

    public void createOrder()
    {
        if (socket != null)
        {
            string data = "{\"product_id\":\"X7SDMI3S4Q2XSH34310UDAF86C5CK2HN\"," 
                + "\"product_name\":\"피자1\","
                + "\"quantity\":1,"
                + "\"product_unit_price\":1000,"
                + "\"paid_price\":1000,"
                + "\"kit01\":1,"
                + "\"is_kitchen\":1,"
                + "\"pos_no\":1,"
                + "\"table_id\":\"AF72010B4B734D7190B16BDB48C6D8A2\","
                + "\"tableName\":\"매장_01\","
                + "\"orderSeq\":101,"
                + "\"is_cancel\":0," + "\"is_tableChanged\":0}";
            Debug.Log("CreateOrder event." + data);
            socket.Emit("createOrder", JSONObject.Create(data));
            msg.text = "이벤트 발생!";
            popup.SetActive(true);
        }
        else
        {
            msg.text = "소켓을 연결하세요!";
            popup.SetActive(true);
        }
    }

    public void tableMove()
    {
        if (socket != null)
        {
            string data = "{\"product_id\":1," + "\"is_cancel\":0," + "\"is_tableChanged\":1," + "\"status\":0\"}";
            Debug.Log("TableMove Event." + data);
            socket.Emit("createOrder", JSONObject.Create(data));
            msg.text = "이벤트 발생!";
            popup.SetActive(true);
        }
        else
        {
            msg.text = "소켓을 연결하세요!";
            popup.SetActive(true);
        }
    }

    public void cancelOrder()
    {
        if (socket != null)
        {
            string data = "{\"old_tablename\":\"매장_01\","
                + "\"new_tablename\":\"매장_02\","
                + "\"pos_no\":1}";
            Debug.Log("CancelOrderEvent." + data);
            socket.Emit("createOrder", JSONObject.Create(data));
            msg.text = "이벤트 발생!";
            popup.SetActive(true);
        }
        else
        {
            msg.text = "소켓을 연결하세요!";
            popup.SetActive(true);
        }
    }

    public void closePopup()
    {
        popup.SetActive(false);
    }

    float time = 0f;
    void FixedUpdate()
    {
        if (!Input.anyKey)
        {
            time += Time.deltaTime;
        }
        else
        {
            if (time != 0f)
            {
                soundObj.Play();
                time = 0f;
            }
        }
    }
}
