using UnityEngine;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections.Generic;
using System.Linq;

namespace Manager_Scripts
{
    public class TwelveButtonList : MonoBehaviour

    {
        public static TwelveButtonList Instance;
        public GameObject[] twelveButtons;
        [SerializeField] private TwelveButtonManager _twelveButtonManager;

        private void Awake()
        {
            _twelveButtonManager.BuscarYOrdenarBotonesPorNombre();

            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }

        }
        void Start()
        {
            InitializeTwelveButtons(_twelveButtonManager.BuscarYOrdenarBotonesPorNombre());
        }

        public void InitializeTwelveButtons(GameObject[] buttons)
        {
            foreach (var button in buttons)
            {
                
            }
        }
    }
}