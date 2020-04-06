using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Unity;
using UnityEngine.UI;

namespace GoFish
{
    public class Game : MonoBehaviour
    {
        public Text MessageText;
        public Text Stack;
        public Text betAmt;
        public Button bet;
        public float currentBet;
        public Slider betSlider;
        public float GamePot;


        protected CardAnimator cardAnimator;

        [SerializeField]
        protected GameDataManager gameDataManager;

        public List<Transform> PlayerPositions = new List<Transform>();
        public List<Transform> BookPositions = new List<Transform>();

        [SerializeField]
        protected Player localPlayer;
        [SerializeField]
        protected Player remotePlayer;

        [SerializeField]
        protected Player currentTurnPlayer;
        [SerializeField]
        protected Player currentTurnTargetPlayer;

        [SerializeField]
        protected Card selectedCard;
        [SerializeField]
        protected Ranks selectedRank;

        public enum GameState
        {
            Idle,
            GameStarted,
            TurnStarted,
            PreFlopAction,
            Bet,
            ConfirmBet,
            WaitingForOpponent,
            Deal,
            GameFinished
        };

        [SerializeField]
        protected GameState gameState = GameState.Idle;

        protected void Awake()
        {
            Debug.Log("base awake");
            localPlayer = new Player();
            localPlayer.PlayerId = "offline-player";
            localPlayer.PlayerName = "Player";
            localPlayer.Position = PlayerPositions[0].position;
            localPlayer.BookPosition = BookPositions[0].position;

            remotePlayer = new Player();
            remotePlayer.PlayerId = "offline-bot";
            remotePlayer.PlayerName = "Bot";
            remotePlayer.Position = PlayerPositions[1].position;
            remotePlayer.BookPosition = BookPositions[1].position;
            remotePlayer.IsAI = true;

            cardAnimator = FindObjectOfType<CardAnimator>();
        }

        protected void Start()
        {
            gameState = GameState.GameStarted;
            GameFlow();
        }

        //****************** Game Flow *********************//
        // 4/5/20 - nned to resolve messages to text windows...

        public virtual void GameFlow()
        {
            if (gameState > GameState.GameStarted)
            {
                CheckPlayersBooks();
                ShowAndHidePlayersDisplayingCards();

                if (gameDataManager.GameFinished())
                {
                    gameState = GameState.GameFinished;
                }
            }

            switch (gameState)
            {
                case GameState.Idle:
                    {
                        Debug.Log("IDLE");
                        break;
                    }
                case GameState.GameStarted:
                    {
                        Debug.Log("GameStarted");
                        currentBet = 0;
                        localPlayer.StackAmt = localPlayer.BuyInAmt;
                        Stack.text = "$" + localPlayer.StackAmt.ToString();
                        OnGameStarted();
                        break;
                    }
                case GameState.TurnStarted:
                    {
                        Debug.Log("TurnStarted");
                        OnTurnStarted();
                        break;
                    }
                case GameState.PreFlopAction:
                    {
                        Debug.Log("Pre-Flop Betting");
                        SetMessage("Pre-flop Betting", localPlayer.StackAmt.ToString());
                        gameState = GameState.Bet;
                        GameFlow();
                        break;
                    }
                case GameState.Bet:
                    {
                        Debug.Log("Bet");
                        PlaceBet();
                        break;
                    }
                    //case GameState.TurnWaitingForOpponentConfirmation:
                    {
                        Debug.Log("TurnWaitingForOpponentConfirmation");
                        OnTurnWaitingForOpponentConfirmation();
                        break;
                    }
                    //case GameState.TurnOpponentConfirmed:
                    {
                        Debug.Log("TurnOpponentConfirmed");
                        OnTurnOpponentConfirmed();
                        break;
                    }
                case GameState.Deal:
                    {
                        Debug.Log("TurnGoFish");
                        OnTurnGoFish();
                        break;
                    }
                case GameState.ConfirmBet:

                case GameState.GameFinished:
                    {
                        Debug.Log("GameFinished");
                        OnGameFinished();
                        break;
                    }
            }
        }

        protected virtual void OnGameStarted()
        {
            gameDataManager = new GameDataManager(localPlayer, remotePlayer);
            //Stack.text = localPlayer.StackAmt.ToString();
            gameDataManager.Shuffle();
            gameDataManager.DealCardValuesToPlayer(localPlayer, Constants.PLAYER_INITIAL_CARDS);
            gameDataManager.DealCardValuesToPlayer(remotePlayer, Constants.PLAYER_INITIAL_CARDS);

            cardAnimator.DealDisplayingCards(localPlayer, Constants.PLAYER_INITIAL_CARDS);
            cardAnimator.DealDisplayingCards(remotePlayer, Constants.PLAYER_INITIAL_CARDS);

            gameState = GameState.TurnStarted;
        }

        protected virtual void OnTurnStarted()
        {
            SwitchTurn();
            gameState = GameState.PreFlopAction;
            GameFlow();
        }

        public void PlaceBet()
        {
            //ResetSelectedCard(); // not needed for hold'em

            if (currentTurnPlayer == localPlayer)
            {
                betSlider.onValueChanged.AddListener(delegate {OnSliderSelected(); });
            }
            else
            {
                SetMessage($"{currentTurnPlayer.PlayerName}'s turn", localPlayer.StackAmt.ToString());
            }

            if (currentTurnPlayer.IsAI)
            {
                //blah
                GameFlow();
            }
        }

        protected virtual void OnTurnConfirmedSelectedNumber()
        {
            if (currentTurnPlayer == localPlayer)
            {
                //SetMessage($"Asking {currentTurnTargetPlayer.PlayerName} for {selectedRank}s...");
            }
            else
            {
                //SetMessage($"{currentTurnPlayer.PlayerName} is asking for {selectedRank}s...");
            }

            gameState = GameState.WaitingForOpponent;
            //TurnWaitingForOpponentConfirmation;
            GameFlow();
        }

        public void OnTurnWaitingForOpponentConfirmation()
        {
            if (currentTurnTargetPlayer.IsAI)
            {
                gameState = GameState.WaitingForOpponent;
                GameFlow();
            }
        }

        protected virtual void OnTurnOpponentConfirmed()
        {
            List<byte> cardValuesFromTargetPlayer = gameDataManager.TakeCardValuesWithRankFromPlayer(currentTurnTargetPlayer, selectedRank);

            if (cardValuesFromTargetPlayer.Count > 0)
            {
                gameDataManager.AddCardValuesToPlayer(currentTurnPlayer, cardValuesFromTargetPlayer);

                bool senderIsLocalPlayer = currentTurnTargetPlayer == localPlayer;
                currentTurnTargetPlayer.SendDisplayingCardToPlayer(currentTurnPlayer, cardAnimator, cardValuesFromTargetPlayer, senderIsLocalPlayer);
                gameState = GameState.PreFlopAction;
            }
            else
            {
                gameState = GameState.Deal;
                GameFlow();
            }
        }

        protected virtual void OnTurnGoFish()
        {
            //SetMessage($"Go fish!");

            byte cardValue = gameDataManager.DrawCardValue();

            if (cardValue == Constants.POOL_IS_EMPTY)
            {
                Debug.LogError("Pool is empty");
                return;
            }

            if (Card.GetRank(cardValue) == selectedRank)
            {
                cardAnimator.DrawDisplayingCard(currentTurnPlayer, cardValue);
            }
            else
            {
                cardAnimator.DrawDisplayingCard(currentTurnPlayer);
                gameState = GameState.TurnStarted;
            }

            gameDataManager.AddCardValueToPlayer(currentTurnPlayer, cardValue);
        }

        public void OnGameFinished()
        {
            if (gameDataManager.Winner() == localPlayer)
            {
                //SetMessage($"You WON!");
            }
            else
            {
                //SetMessage($"You LOST!");
            }
        }

        //****************** Helper Methods *********************//
        public void ResetSelectedCard()
        {
            if (selectedCard != null)
            {
                selectedCard.OnSelected(false);
                selectedCard = null;
                selectedRank = 0;
            }
        }

        protected void SetMessage(string message, string stack_update)
        {
            MessageText.text = message;
            Stack.text = stack_update;
        }

        public void SwitchTurn()
        {
            if (currentTurnPlayer == null)
            {
                currentTurnPlayer = localPlayer;
                currentTurnTargetPlayer = remotePlayer;
                return;
            }

            if (currentTurnPlayer == localPlayer)
            {
                currentTurnPlayer = remotePlayer;
                currentTurnTargetPlayer = localPlayer;
            }
            else
            {
                currentTurnPlayer = localPlayer;
                currentTurnTargetPlayer = remotePlayer;
            }
        }

        public void PlayerShowBooksIfNecessary(Player player)
        {
            Dictionary<Ranks, List<byte>> books = gameDataManager.GetBooks(player);

            if (books != null)
            {
                foreach (var book in books)
                {
                    player.ReceiveBook(book.Key, cardAnimator);

                    gameDataManager.RemoveCardValuesFromPlayer(player, book.Value);
                    gameDataManager.AddBooksForPlayer(player, book.Key);
                }
            }
        }

        public void CheckPlayersBooks()
        {
            List<byte> playerCardValues = gameDataManager.PlayerCards(localPlayer);
            localPlayer.SetCardValues(playerCardValues);
            PlayerShowBooksIfNecessary(localPlayer);

            playerCardValues = gameDataManager.PlayerCards(remotePlayer);
            remotePlayer.SetCardValues(playerCardValues);
            PlayerShowBooksIfNecessary(remotePlayer);
        }

        public void ShowAndHidePlayersDisplayingCards()
        {
            localPlayer.ShowCardValues();
            remotePlayer.HideCardValues();
        }

        //****************** User Interaction *********************//
        public void OnCardSelected(Card card)
        {
            if (gameState == GameState.Bet)
            {
                if (card.OwnerId == currentTurnPlayer.PlayerId)
                {
                    if (selectedCard != null)
                    {
                        selectedCard.OnSelected(false);
                        selectedRank = 0;
                    }

                    selectedCard = card;
                    selectedRank = selectedCard.Rank;
                    selectedCard.OnSelected(true);
                    //SetMessage($"Ask {currentTurnTargetPlayer.PlayerName} for {selectedCard.Rank}s ?");
                }
            }
        }

        public virtual void OnBetSelected(float sliderVal)
        {
            if (gameState == GameState.Bet && localPlayer == currentTurnPlayer)
            {
                if (betSlider != null)
                {
                    currentBet = sliderVal;
                    gameState = GameState.ConfirmBet;
                    GameFlow();
                }
            }
            else if (gameState == GameState.WaitingForOpponent && localPlayer == currentTurnTargetPlayer)
            {
                gameState = GameState.WaitingForOpponent;
                GameFlow();
            }
        }

        public virtual void OnSliderSelected()
        {
            Debug.Log("in BetSelected");
            Debug.Log("GameState =" + gameState);

            if (gameState == GameState.Bet && localPlayer == currentTurnPlayer)
            {
                betSlider.minValue = currentBet;
                betSlider.maxValue = localPlayer.StackAmt;
                Debug.Log(betSlider.minValue);
                Debug.Log(betSlider.maxValue);
                float betVal = betSlider.value;
                betAmt.text ="$" + betVal.ToString();
                bet.onClick.AddListener(delegate { OnBetSelected(betVal); });
            }
        }


        //****************** Animator Event *********************//
        public virtual void AllAnimationsFinished()
        {
            GameFlow();
        }
    }
}
