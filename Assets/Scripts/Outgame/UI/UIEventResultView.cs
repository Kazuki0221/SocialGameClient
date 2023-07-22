using Cysharp.Threading.Tasks;
using MD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

namespace Outgame
{
    public class UIEventQuestResultView : UIStackableView
    {
        [SerializeField] GameObject _root;
        [SerializeField] GameObject _rewardPrefab;

        int _questId = 0;

        protected override void AwakeCall()
        {
            ViewId = ViewID.EventResult;
            _hasPopUI = false;

            CreateView();
        }

        string GetRewardObjectString(APIResponceQuestReward reward)
        {
            string ret = "";
            switch ((RewardItemType)reward.type)
            {
                case RewardItemType.None: break;
                case RewardItemType.Card: ret = MasterData.GetLocalizedText(MasterData.GetCard(int.Parse(reward.param[0])).Name); break;
                case RewardItemType.Money: ret = string.Format("{0}Money", int.Parse(reward.param[0])); break;
                case RewardItemType.Item: ret = string.Format("{0}{1}��", MasterData.GetLocalizedText(MasterData.GetItem(int.Parse(reward.param[0])).Name), int.Parse(reward.param[1])); break;

                    //TODO: �C�x���g�|�C���g
                    //case RewardItemType.EventPoint: ret = string.Format("{0}�|�C���g", int.Parse(reward.param[0])); break;
            }
            return ret;
        }

        void CreateView()
        {
            var package = SequenceBridge.GetSequencePackage<QuestPackage>("Quest");

            foreach (var reward in package?.QuestResult?.rewards)
            {
                Debug.Log(reward);
                if (reward.type == 0) continue;

                var rewardObj = GameObject.Instantiate(_rewardPrefab, _root.transform);
                var text = rewardObj.GetComponent<TextMeshProUGUI>();

                text.text = string.Format("{0}����ɓ��ꂽ", GetRewardObjectString(reward));
            }

            SequenceBridge.DeleteSequence("Quest");
        }

        public void GoHome()
        {
            UIManager.NextView(ViewID.EventHome);
        }
    }
}
