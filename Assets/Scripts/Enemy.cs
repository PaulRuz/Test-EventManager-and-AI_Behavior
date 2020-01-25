using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProjectA.Interfaces;

namespace ProjectA
{

    public class Enemy : MonoBehaviour, ISetDamage, IListener
    {
        public int ID {get; private set;}
        public string Name { get; private set; }
        public int Health { get; set; } = 100;

        private bool _isDead = false;

        private void Start() {
            EventManager.Instance.AddListener(EVENT_TYPE.HEALTH_CHANGE, this);
        }

        private void Update() {
            if(!_isDead && Input.GetKeyDown(KeyCode.Space)) {
                ApplyDamage(10);
                return;
            }
        }

        public void ApplyDamage(int damage) {
            if(Health > 0) {
                Health -= damage;
                EventManager.Instance.PostNotification(EVENT_TYPE.HEALTH_CHANGE, this, Health);
            }
            else if (Health <= 0) {
                Health = 0;
                _isDead = true;
            }

        }

        public void OnEvent(EVENT_TYPE event_type, Component sender, object param = null) {
            switch(event_type){
                case EVENT_TYPE.HEALTH_CHANGE:
                    OnHealthChange(sender, (int)param);
                    break;
            }
        }

        private void OnHealthChange(Component enemy, int newHealth) {
            if(enemy.GetInstanceID() != this.GetInstanceID()) return;
            Debug.Log("Object: " + gameObject.name + " | Health: " + Health.ToString());
        }
    }
}

