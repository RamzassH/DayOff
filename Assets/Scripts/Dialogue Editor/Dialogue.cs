using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace RPG.Dialogue
{
    [CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue", order = 0)]
    public class Dialogue : ScriptableObject, ISerializationCallbackReceiver
    {
        [SerializeField] private List<DialogueNode> nodes = new List<DialogueNode>();
        [SerializeField] private Vector2 newNodeOffset = new Vector2(250, 0);

        Dictionary<string, DialogueNode> nodeLookup = new Dictionary<string, DialogueNode>();

        private void OnValidate()
        {
            nodeLookup.Clear();
            if (nodes.Count == 0) 
            {
                return;
            }
            foreach (DialogueNode node in GetAllNodes())
            {
                nodeLookup[node.name] = node;
            }
        }

        public DialogueNode GetRootNode() {
            return nodes[0];
        }

        public IEnumerable<DialogueNode> GetAllNodes() {
            return nodes;
        }

        internal IEnumerable<DialogueNode> GetAllChildren(DialogueNode parentNode)
        {
            List<DialogueNode> result = new List<DialogueNode>();
            foreach (string childID in parentNode.GetChildren()) {
                if (nodeLookup.ContainsKey(childID))
                {
                    yield return nodeLookup[childID];
                }
            }
        }

#if UNITY_EDITOR

        public void CreateNode(DialogueNode parent)
        {
            DialogueNode newNode = MakeNode(parent);
            Undo.RegisterCreatedObjectUndo(newNode, "Created Dialogue Node");
            Undo.RecordObject(this, "Added Dialogue Node");
            AddNode(newNode);
        }

        public void DeleteNode(DialogueNode nodeDelete)
        {
            Undo.RecordObject(this, "Deleted Dialogue Node");
            nodes.Remove(nodeDelete);
            OnValidate();
            CleanDanglingChildren(nodeDelete);
            Undo.DestroyObjectImmediate(nodeDelete);
        }

        private DialogueNode MakeNode(DialogueNode parent)
        {
            DialogueNode newNode = CreateInstance<DialogueNode>();
            newNode.name = Guid.NewGuid().ToString();
            if (parent != null)
            {
                parent.AddChild(newNode.name);
                newNode.SetIsPlayerSpeaking(!parent.IsPlayerSpeaking);
                newNode.SetPosition(parent.GetRect().position + newNodeOffset);
            }

            return newNode;
        }

        private void AddNode(DialogueNode newNode)
        {
            nodes.Add(newNode);
            OnValidate();
        }

        private void CleanDanglingChildren(DialogueNode nodeDelete)
        {
            foreach (DialogueNode node in GetAllNodes())
            {
                node.RemoveChild(nodeDelete.name);
            }
        }
#endif
        public void OnBeforeSerialize()
        {
#if UNITY_EDITOR
            if (nodes.Count == 0)
            {
                DialogueNode newNode = MakeNode(null);
                AddNode(newNode);
            }

            if (AssetDatabase.GetAssetPath(this) != "") 
            {
                foreach (DialogueNode node in GetAllNodes()) 
                {
                    if (AssetDatabase.GetAssetPath(node) == "") 
                    {
                        AssetDatabase.AddObjectToAsset(node, this);
                    }
                }
            }
#endif
        }

        public void OnAfterDeserialize()
        {

        }
    }
}