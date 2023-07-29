using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


namespace Outgame
{
    public class UIEventHomeView : UIStackableView
    {
        protected override void AwakeCall()
        {
            ViewId = ViewID.EventHome;
            _hasPopUI = true;

        }

        public override void Enter()
        {
            base.Enter();

            UIStatusBar.Show();

        }

        public void GoHome()
        {
            UIManager.NextView(ViewID.Home);
        }

        public void GoRanking()
        {
            UIManager.NextView(ViewID.Ranking);
        }

        public void GoEventQuest()
        {
            if (EventHelper.IsEventGamePlayable(UIHomeView.evtId))
            {
                UIManager.NextView(ViewID.EventQuest);
            }
        }

        public void DialogTest()
        {
            UICommonDialog.OpenOKDialog("テスト", "テストダイアログですよ", Test);
        }

        void Test(int type)
        {
            Debug.Log("here");
        }
    }
}
