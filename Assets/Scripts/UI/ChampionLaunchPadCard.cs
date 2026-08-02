using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DragonSigil.UI
{
    /// <summary>
    /// A single draggable champion card in the launch pad. Spawns a
    /// cursor-following ghost while dragging and, on release, asks the
    /// shared LaunchPadController to deploy this card's champion wherever
    /// the pointer landed.
    /// </summary>
    [RequireComponent(typeof(Image))]
    public class ChampionLaunchPadCard : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] private GameObject championPrefab;
        [SerializeField] private LaunchPadController launchPadController;

        private RectTransform _ghost;

        public void OnBeginDrag(PointerEventData eventData)
        {
            var icon = GetComponent<Image>();

            var ghostGO = new GameObject("ChampionDragGhost", typeof(RectTransform), typeof(Image));
            ghostGO.transform.SetParent(GetComponentInParent<Canvas>().transform, worldPositionStays: false);

            var ghostImage = ghostGO.GetComponent<Image>();
            ghostImage.sprite = icon.sprite;
            ghostImage.color = icon.color;
            ghostImage.raycastTarget = false;

            _ghost = ghostGO.GetComponent<RectTransform>();
            _ghost.sizeDelta = ((RectTransform)transform).sizeDelta;
            MoveGhostTo(eventData.position);
        }

        public void OnDrag(PointerEventData eventData)
        {
            MoveGhostTo(eventData.position);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (_ghost != null)
            {
                Destroy(_ghost.gameObject);
                _ghost = null;
            }

            launchPadController.TryPlaceChampion(championPrefab, eventData.position);
        }

        private void MoveGhostTo(Vector2 screenPosition)
        {
            _ghost.position = new Vector3(screenPosition.x, screenPosition.y, 0f);
        }
    }
}
