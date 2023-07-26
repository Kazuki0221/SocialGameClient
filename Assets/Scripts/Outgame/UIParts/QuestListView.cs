﻿using MD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Outgame
{
    /// <summary>
    /// クエストリストを表示するビュー
    /// </summary>
    public class QuestListView : ListView
    {
        public delegate void Ready(int questId);
        [SerializeField] GameObject _chapterPrefab;
        [SerializeField] GameObject _questStackPrefab;
        [SerializeField] GameObject _questPrefab;

        public enum BoardType
        {
            Chapter,
            Quest
        }
        List<List<GameObject>> _childList = new List<List<GameObject>>();
        List<ListItemQuestBoard> _questList = new List<ListItemQuestBoard>();
        Ready _callback;
        int _selectedQuestId = -1;


        /// <summary>
        /// ビューを作る
        /// </summary>
        public override void Setup()
        {
            _lineList.ForEach(l => GameObject.Destroy(l));
            _itemList.Clear();
            _scrollPos = 0;

            var chapters = MasterData.Chapters;
            var questList = QuestListModel.QuestList.List;
            var pastview = UIManager.BeforeView();

            //Debug.Log(chapters[4].QuestList[0].Id);
            //チャプターとその子供になるクエストをリストに入れる
            for (int i = 0; i < chapters.Count; ++i)
            {
                if(pastview == ViewID.Home && chapters[i].QuestType != 0)
                {
                    continue;
                }
                else if(pastview == ViewID.EventHome && chapters[i].QuestType != 1)
                {
                    continue;
                }
                var chapter = GameObject.Instantiate(_chapterPrefab, _content.RectTransform);
                var listItem = ListItemBase.ListItemSetup<ListItemChapterBoard>(i, chapter, (int evtId, int index) => OnItemClick(evtId, index));
                listItem.SetupChapterData(chapters[i]);

                _itemList.Add(listItem);
                _lineList.Add(listItem.gameObject);

                _childList.Add(new List<GameObject>());
                Debug.Log($"{i},{chapters[i].QuestList.Count}");

                //クエストは非表示で作る
                for (int q = 0; q < chapters[i].QuestList.Count; ++q)
                {
                    Debug.Log(questList.Where(qi => qi.QuestId == chapters[i].QuestList[q].Id).FirstOrDefault());
                    var quest = GameObject.Instantiate(_questPrefab, _content.RectTransform);
                    var listItem2 = ListItemBase.ListItemSetup<ListItemQuestBoard>(_questList.Count, quest, (int evtId, int index) => OnItemClick(evtId, index));
                    listItem2.SetupQuestData(chapters[i].QuestList[q].Id, questList.Where(qi => qi.QuestId == chapters[i].QuestList[q].Id).FirstOrDefault());
                    listItem2.gameObject.SetActive(false);

                    _questList.Add(listItem2);
                    _itemList.Add(listItem2);
                    _lineList.Add(listItem2.gameObject);

                    _childList[i].Add(listItem2.gameObject);
                }
            }

            //サイズ計算して最大スクロール値を決める
            //クエストはサイズ可変するので毎回再計算する
            _content.RectTransform.sizeDelta = new Vector2(800, (_lineList.Where(go => go.activeSelf).Count() + 1) * CardUIHeight);

            //イベント登録
            _rect.onValueChanged.AddListener(ScrollUpdate);
        }



        public void SetReadyCallback(Ready cb)
        {
            _callback = cb;
        }

        protected override void OnItemClick(int evtId, int index)
        {
            //Debug.Log($"{evtId}, {index}");
            switch((BoardType)evtId)
            {
                //チャプターを押した場合はクエストを出す
                case BoardType.Chapter:
                    {
                        //非表示切替
                        _childList[index].ForEach(c => c.SetActive(!c.activeSelf));

                        //サイズ計算して最大スクロール値を決める
                        //クエストはサイズ可変するので毎回再計算する
                        _content.RectTransform.sizeDelta = new Vector2(800, (_lineList.Where(go => go.activeSelf).Count() + 1) * CardUIHeight);
                    }
                    break;

                //出撃確認
                case BoardType.Quest:
                    {
                        _selectedQuestId = _questList[index].QuestId;
                        UICommonDialog.OpenYesNoDialog("出撃します", "よかったらOK", DialogDecide, "UIGoQuest", "UINoQuest");
                    }
                    break;
            }
        }

        void DialogDecide(int type)
        {
            if (type == 1)
            {
                _callback?.Invoke(_selectedQuestId);
            }
        }
    }
}
