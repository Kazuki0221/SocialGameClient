using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;


namespace Outgame
{
    public class UIHomeView : UIStackableView
    {
        [SerializeField] List<GameObject> eventButton; //イベントボタンリスト

        public static int evtId = 0;

        protected override void AwakeCall()
        {
            ViewId = ViewID.Home;
            _hasPopUI = true;

            foreach (var evt in EventHelper.GetAllOpenedEvent())
            {
                if (EventHelper.IsEventOpen(evt))
                {
                    //開催中のイベントを表示
                    var button = eventButton[evt - 1];
                    button.SetActive(true);
                    button.GetComponent<Button>().onClick.AddListener(() => SetEventID(evt));
                }
            }

        }

        public override void Enter()
        {
            base.Enter();

            UIStatusBar.Show();

            Debug.Log(EventHelper.GetAllOpenedEvent());
            Debug.Log(EventHelper.IsEventOpen(1));
            Debug.Log(EventHelper.IsEventGamePlayable(1));
        }

        public void GoGacha()
        {
            UIManager.NextView(ViewID.Gacha);
        }

        public void GoCardList()
        {
            UIManager.NextView(ViewID.CardList);
        }

        public void GoEnhance()
        {
            UIManager.NextView(ViewID.Enhance);
        }

        public void GoQuest()
        {
            UIManager.NextView(ViewID.Quest);
        }

        public void GoEvent()
        {
            UIManager.NextView(ViewID.EventHome);
        }

        public void OpenInformation()
        {
            UIManager.StackView(ViewID.Information);
        }



        public void DialogTest()
        {
            UICommonDialog.OpenOKDialog("テスト", "テストダイアログですよ", Test);
        }

        void Test(int type)
        {
            Debug.Log("here");
        }

        /// <summary>
        /// 選択したイベントのIDを保存
        /// </summary>
        /// <param name="id"></param>
        void SetEventID(int id)
        {
            evtId = id;
        }
    }
}
