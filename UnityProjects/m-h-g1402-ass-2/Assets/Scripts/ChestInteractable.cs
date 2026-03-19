using DG.Tweening;
using Interfaces;
using UI;
using UnityEngine;

public class ChestInteractable : MonoBehaviour, IInteractable
{
   private static readonly int IsOpen = Animator.StringToHash("IsOpen");
   [SerializeField] private Animator anim;
   private Tween loopTween;
   //This field 'collectTween' IS being used, Intellisense is being silly
   private Tween collectTween;


   private void Start()
   {
      if (!anim) return;
      
      transform.DOScale(1.2f, .5f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutQuad);
   }


   public void OnHoverIn()
   {
      if (anim != null)
      {
         Debug.Log("Interactor In");
         anim?.SetBool(IsOpen, true);

         //TODO - Show UI
         Toast.Instance.ShowToast("Press \"E\" to Interact");
      }
   }

   public void OnHoverOff()
   {
      if (anim != null)
      {

         Debug.Log("Interactor Off");
         anim?.SetBool(IsOpen, false);
         //TODO - Hide UI
         Toast.Instance.HideToast();
      }
   }

   public void OnInteract()
   {
      if (anim != null)
      {
         Debug.Log($"Interacted with {gameObject.name}");

         collectTween = transform.DOScale(0, .5f).SetEase(Ease.InBack).OnComplete(() => { Destroy(gameObject); });
      }
   }

   void OnDestroy()
   {
      transform.DOKill();
   }
}
