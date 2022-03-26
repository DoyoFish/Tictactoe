using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UIBinding
{

    public class UGUIOnClickBinding : ActionBinding
    {
        public bool Block;
        public Button Button;
        public float ClickInterval = 0.5f;

        public static Action<MusicEnum> MusicEffect;

        public MusicEnum SingleMusicEffect = MusicEnum.Click;

        private float _lastClick;

        private void Reset()
        {
            base.OnValidate();
            if (!Button)
            {
                Button = GetComponent<Button>();
            }
        }

        private void Awake()
        {
            OnValidate();
            if (!Button)
            {
                Button = this.GetComponent<Button>();
            }
            if (Button)
            {
                Button.onClick.AddListener(OnClick);
            }
            else
            {
                Debug.LogError("√ª”–’“µΩButton");
            }
        }

        private void OnClick()
        {
            var onClick = CreateAction();
            if (onClick != null && !Block)
            {
                var interval = Time.realtimeSinceStartup - _lastClick;
                if (interval < ClickInterval)
                {
                    return;
                }
                _lastClick = Time.realtimeSinceStartup;
                MusicEffect.Invoke(SingleMusicEffect);

                onClick.Invoke();
            }
        }

        protected override void OnValidate()
        {
            base.OnValidate();
            if (!Button)
            {
                Button = this.GetComponent<Button>();
            }
        }
    }

}