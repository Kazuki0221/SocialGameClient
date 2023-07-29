using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

namespace Outgame
{
    public class UIRankingView : UIStackableView
    {

        protected override void AwakeCall()
        {
            ViewId = ViewID.Ranking;
            _hasPopUI = true;
        }



        public void GoEvent()
        {
            UIManager.NextView(ViewID.EventHome);
        }

    }
}
