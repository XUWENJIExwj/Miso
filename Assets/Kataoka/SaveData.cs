using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NCMB;

namespace NCMB
{
    public class SaveData
    {

        public string savedata { get; set; }
        public string name { get; private set; }

        // コンストラクタ -----------------------------------
        public SaveData(string _savedata, string _name)
        {
            savedata = _savedata;
            name = _name;
        }

        // サーバーにセーブデータを保存 -------------------------
        public void save()
        {
            // データストアの「SaveData」クラスから、Nameをキーにして検索
            NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>("SaveData");
            query.WhereEqualTo("Name", name);
            query.FindAsync((List<NCMBObject> objList, NCMBException e) => {

                //検索成功したら
                if (e == null)
                {
                    objList[0]["Data"] = savedata;
                    objList[0].SaveAsync();
                }
            });
        }

        // サーバーからハイスコアを取得  -----------------
        public void fetch()
        {
            // データストアの「SaveData」クラスから、Nameをキーにして検索
            NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>("SaveData");
            query.WhereEqualTo("Name", name);
            query.FindAsync((List<NCMBObject> objList, NCMBException e) => {

                //検索成功したら
                if (e == null)
                {
                    // ハイスコアが未登録だったら
                    if (objList.Count == 0)
                    {
                        NCMBObject obj = new NCMBObject("SaveData");
                        obj["Name"] = name;
                        obj["Data"] = "";
                        obj.SaveAsync();
                        savedata = "";
                    }
                    // ハイスコアが登録済みだったら
                    else
                    {
                        savedata = System.Convert.ToString(objList[0]["Data"]);
                       
                    }
                }
            });
        }

    }
}