using System;
using System.Collections.Generic;
using System.Text;
using Edu.Entity;
using Edu.Entity.SchoolFinance;

namespace Edu.BLL.SchoolFinance
{
    /// <summary>
    /// set trial duration freely
    /// password trying times limited.
    /// customer serviceman telephone.
    /// finance card and yearcard both are freely activated.
    /// and the expireday is the durationg adding the activating day .
    /// </summary>
    public sealed class CardUse:StudyCard
    {
        private readonly string _userid;
        //private AppConfigs.BatchType _cardType;
        public enum ActivateResult
        {
            cardNotExist,
            cardFreezed,
            ActivateSuccess,
            ActivateFailed
        }
        public CardUse(string userid)
        {
            _userid = userid;
        }

      


        /// <summary>
        /// activated card by id
        /// write the card end day
        ///  5 times trial,
        ///  user has only one card.
        /// </summary>
        /// <param name="CardId"></param>
        /// <returns>action result</returns>
        public ActivateResult ActivateCard(string FinCardId,string pwd)
        {
            var card = CardExist(FinCardId);
            if (card != null)
            {
                // card already in use.
                if (card.Status == AppConfigs.SingleCardStatus.InUse)
                {
                    return ActivateResult.ActivateSuccess;
                }
                else
                {
                    if (card.LockedEndTime.HasValue)
                    {
                        if (card.LockedEndTime >= DateTime.Now)
                        {
                            return ActivateResult.cardFreezed;
                        }
                        else
                        {
                            card.FailTimes = 0;
                            card.LockedEndTime = null;
                            _BLL.Update(card);
                        }
                    }

                    int i;
                    if (TryActivate(FinCardId, pwd, out i))
                    {
                        card.FailTimes = 0;
                        card.ActivatedDay = DateTime.Now.ToString();
                        card.LockedEndTime =null;
                        card.Status = AppConfigs.SingleCardStatus.InUse;
                        card.StatusDay=DateTime.Now;
                        card.UserId = _userid;
                        card.EndDay=DateTime.Now.AddDays((double)new FINCardConfigBLL().SingleCardConfig(card.CardConfigId).ValidPeriod);
                        _BLL.Update(card);
                        return ActivateResult.ActivateSuccess;
                    }
                    else
                    {
                        card.FailTimes += 1;
                        if (card.FailTimes >4)
                        {
                            card.LockedEndTime=DateTime.Now.AddMinutes(5);
                            _BLL.Update(card);
                            return ActivateResult.cardFreezed;
                        }

                        _BLL.Update(card);
                        return ActivateResult.ActivateFailed;
                    }
                }
            }
            else
            {
                return ActivateResult.cardNotExist;
            }
        
        }



        /// <summary>
        /// try to active a card with pwd.
        /// </summary>
        /// <param name="cardid"></param>
        /// <param name="pwd"></param>
        /// <param name="failed"></param>
        /// <returns></returns>
        private bool TryActivate(string cardid, string pwd,out int failed)
        {
            return base._BLL.TryActivate(cardid, pwd,out failed);
        }


        private FINCard CardExist(string cardid)
        {
            return _BLL.Single(cardid);
        }

    }
}
