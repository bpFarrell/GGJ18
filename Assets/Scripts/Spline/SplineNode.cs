/*
////////////////////////////////////////////////////////////////////
 Jason Dean Crossley

 Notes:
\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
*/

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SplineLogic
{
    public class SplineNode : MonoBehaviour
    {
        public int id;
        public float size;
        public Transform _enterHandle;
        public Vector3 enter
        {
            get
            {
                if (_enterHandle == null)
                {
                    _enterHandle = transform.Find("enter handle");
                    if (_enterHandle == null)
                        return Vector3.zero;
                    else
                        return _enterHandle.position;
                }
                else return _enterHandle.position;
            }
        }

        public Transform _exitHandle;

        public Vector3 exit
        {
            get
            {
                if (_exitHandle == null)
                {
                    _exitHandle = transform.Find("exit handle");
                    if (_exitHandle == null)
                        return Vector3.zero;
                    else
                        return _exitHandle.position;
                }
                else return _exitHandle.position;
            }
        }

        public Vector3 node
        {
            get { return transform.position; }
        }
        private void Start()
        {
            _enterHandle = transform.Find("enter handle");
            _exitHandle = transform.Find("exit handle");
        }
        public void Init()
        {
            if (_enterHandle == null)
            {
                _enterHandle = new GameObject("enter handle").transform;
                _enterHandle.parent = transform;
                _enterHandle.localPosition = (-1f * Vector3.forward);
                _enterHandle.rotation = Quaternion.identity;
                _enterHandle.localScale = Vector3.one;
                SplineHandle enterhand = _enterHandle.gameObject.AddComponent<SplineHandle>();
                enterhand.size = size / 3;
                enterhand.color = Color.cyan;
                
            }
            if (_exitHandle == null)
            {
                _exitHandle = new GameObject("exit handle").transform;
                _exitHandle.parent = transform;
                _exitHandle.localPosition = (1f * Vector3.forward);
                _exitHandle.rotation = Quaternion.identity;
                _exitHandle.localScale = Vector3.one;
                SplineHandle exithand = _exitHandle.gameObject.AddComponent<SplineHandle>();
                exithand.size = size / 3;
                exithand.color = Color.blue;
            }
        }
        private void OnDrawGizmos()
        {
            DrawHandle(size);
        }
        public void DrawHandle(float size)
        {
            _exitHandle.GetComponent<SplineHandle>().DrawHandle(size / 3);
            _enterHandle.GetComponent<SplineHandle>().DrawHandle(size / 3);
            Color prevColor = Gizmos.color;
            Gizmos.DrawLine(transform.position, enter);
            Gizmos.DrawLine(transform.position, exit);
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(transform.position, size);
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, transform.position + transform.up);
            Gizmos.color = prevColor;
        }
    }
}