using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using ProjectA.Interfaces;

namespace ProjectA
{
    public enum EVENT_TYPE { GAME_END, HEALTH_CHANGE, DEAD };
    
    public class EventManager : MonoBehaviour {
        public static EventManager Instance {get; private set;}
        private Dictionary<EVENT_TYPE, List<IListener>> Listeners =
            new Dictionary<EVENT_TYPE, List<IListener>>();
        

        private void OnEnable() {
            SceneManager.sceneLoaded += OnLevelWasLoaded;
        }
        private void Awake() {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
                DestroyImmediate(this);
        }

        public void AddListener(EVENT_TYPE event_type, IListener listener) {
            
            List<IListener> ListenList = null;
            if(Listeners.TryGetValue(event_type, out ListenList)) {
                ListenList.Add(listener);
                return;
            }
            ListenList = new List<IListener>();
            ListenList.Add(listener);
            Listeners.Add(event_type, ListenList);
        }

        public void PostNotification(EVENT_TYPE event_type, Component sender, object param) {
            List<IListener> ListenList = null;
            if(!Listeners.TryGetValue(event_type, out ListenList)) {
                return;
            }
            for(int i = 0; i < ListenList.Count; i++) {
                if(!ListenList[i].Equals(null))
                    ListenList[i].OnEvent(event_type, sender, param);
            }
        }

        public void RemoveEvent(EVENT_TYPE event_type) {
            Listeners.Remove(event_type);
        }

        public void RemoveRedundancies() {
            Dictionary<EVENT_TYPE, List<IListener>> tempListeners = 
                new Dictionary<EVENT_TYPE, List<IListener>>();
            foreach(KeyValuePair<EVENT_TYPE, List<IListener>> Item in Listeners) {
                for(int i = Item.Value.Count-1; i >= 0; i--) {
                    if(Item.Value[i].Equals(null))
                        Item.Value.RemoveAt(i);
                }

                if(Item.Value.Count > 0) 
                    tempListeners.Add(Item.Key, Item.Value);
            }
            Listeners = tempListeners;
        }


        private void OnDisable() {
            SceneManager.sceneLoaded -= OnLevelWasLoaded;
        }
        private void OnLevelWasLoaded(Scene scene, LoadSceneMode mode) {
            RemoveRedundancies();
        }

    }
   
}

