using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using LuaInterface;

namespace Uppower
{
    public class UIContext : MonoBehaviour
    {
        public enum ScriptType
        {
            Lua         = 1,
            CScript     = 2,
        }

        public ScriptType scriptType = ScriptType.Lua;
        public string scriptPath;
        List<string> _functions;
        List<string> _properties;

        void Start()
        {
            if (scriptType == ScriptType.Lua) {
                FileInfo file = new FileInfo(scriptPath);
                FileStream stream = file.OpenRead();
                byte[] text = new byte[file.Length];
                stream.Read(text, 0, (int)file.Length);

                LuaState lua = new LuaState(); lua.Start();
                lua.DoString(System.Text.Encoding.Default.GetString(text));

                string tableName = Path.GetFileNameWithoutExtension(scriptPath);
                LuaTable luaTable = lua.GetTable(tableName);
                if (luaTable != null) {
                    LuaDictTable dict = luaTable.ToDictTable();
                    var iter = dict.GetEnumerator();
                    while (iter.MoveNext()) {
                        if (iter.Current.Value is LuaFunction) {
                            _functions.Add(iter.Current.Key as string);
                        } else if((iter.Current.Value as string) != null) {
                            _properties.Add(iter.Current.Key as string);
                        }
                    }
                }
            } else {
                
            }
        }
    }
}