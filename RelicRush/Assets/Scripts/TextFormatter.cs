using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    [RequireComponent(typeof(Text))]
    public class TextFormatter : MonoBehaviour
    {
        private string formatString;
        private IDictionary<string, string> mapping;

        public void Awake()
        {
            mapping = new Dictionary<string, string>();
            formatString = GetComponent<Text>().text;
        }

        public void Start()
        {
            RefreshText();
        }

        public void Set(string key, object value)
        {
            mapping[key] = value.ToString();
            RefreshText();
        }

        private void RefreshText()
        {
            var workingString = formatString;
            foreach (var pair in mapping)
            {
                workingString = workingString.Replace("{" + pair.Key + "}", pair.Value);
            }

            workingString = Regex.Replace(workingString, @"{\w}", "");

            GetComponent<Text>().text = workingString;
        }
    }
}