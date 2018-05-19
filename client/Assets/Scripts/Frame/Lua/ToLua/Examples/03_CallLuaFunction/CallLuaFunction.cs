using UnityEngine;
using System.Collections;
using LuaInterface;
using System;

public class CallLuaFunction : MonoBehaviour 
{
    private string script =
        @"  function luaFunc(num)                        
                print(num)
                return num + 1
            end

            test = {}
            test.luaFunc = luaFunc
            test.callback = luaFunc

            test.name = '123'
            test.value1 = 1
            test.value2 = 2
        ";

    LuaFunction luaFunc = null;
    LuaState lua = null;
    string tips = null;
	
	void Start () 
    {
#if UNITY_5 || UNITY_2017
        Application.logMessageReceived += ShowTips;
#else
        Application.RegisterLogCallback(ShowTips);
#endif
        new LuaResLoader();
        lua = new LuaState();
        lua.Start();
        DelegateFactory.Init();        
        lua.DoString(script);

        //Get the function object
        luaFunc = lua.GetFunction("test.luaFunc");

        if (luaFunc != null)
        {
            int num = luaFunc.Invoke<int, int>(123456);
            Debugger.Log("generic call return: {0}", num);

            num = CallFunc();
            Debugger.Log("expansion call return: {0}", num);

            Func<int, int> Func = luaFunc.ToDelegate<Func<int, int>>();
            num = Func(123456);
            Debugger.Log("Delegate call return: {0}", num);
            
            num = lua.Invoke<int, int>("test.luaFunc", 123456, true);
            Debugger.Log("luastate call return: {0}", num);
        }

        LuaTable luaTable = lua.GetTable("test");
        luaTable.Call<int>("luaFunc", 1000);
        LuaDictTable dict = luaTable.ToDictTable();
        var iter2 = dict.GetEnumerator();

        while (iter2.MoveNext())
        {
            Debugger.Log("map item, k,v is {0}:{1}", iter2.Current.Key, iter2.Current.Value);
        }

        iter2.Dispose();
        dict.Dispose();
        luaTable.Dispose();

        lua.CheckTop();
	}

    void ShowTips(string msg, string stackTrace, LogType type)
    {
        tips += msg;
        tips += "\r\n";
    }

#if !TEST_GC
    void OnGUI()
    {
        GUI.Label(new Rect(Screen.width / 2 - 200, Screen.height / 2 - 150, 400, 300), tips);
    }
#endif

    void OnDestroy()
    {
        if (luaFunc != null)
        {
            luaFunc.Dispose();
            luaFunc = null;
        }

        lua.Dispose();
        lua = null;

#if UNITY_5 || UNITY_2017
        Application.logMessageReceived -= ShowTips;
#else
        Application.RegisterLogCallback(null);
#endif
    }

    int CallFunc()
    {        
        luaFunc.BeginPCall();                
        luaFunc.Push(123456);
        luaFunc.PCall();        
        int num = (int)luaFunc.CheckNumber();
        luaFunc.EndPCall();
        return num;                
    }
}
