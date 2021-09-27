using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace ZombieTiles.Mechanics.Building
{
    public class Builder : MonoBehaviour
    {
        private int selection = 0;

        private const string SelectPreviousBuildingButtonName = "Select Previous Building";
        private const string SelectNextBuildingButtonName = "Select Next Building";

        [SerializeField]
        private List<Buildable> buildings;

        private void Update() =>
            selection = (selection +
                Convert.ToInt32(Input.GetButtonDown(SelectNextBuildingButtonName)) -
                Convert.ToInt32(Input.GetButtonDown(SelectPreviousBuildingButtonName)) +
                buildings.Count) %
                buildings.Count;

        private void Start()
        {
            Assert.IsNotNull(buildings);
        }

        public void RequestBuildOn(BuildBase buildBase)
        {
            Instantiate(buildings[selection],
                buildBase.transform.position + new Vector3(0, 1),
                buildBase.transform.rotation);
        }
    }
}