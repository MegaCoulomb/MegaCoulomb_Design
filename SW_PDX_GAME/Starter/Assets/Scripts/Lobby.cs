﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using SWNetwork;

namespace GoFish
{
    public class Lobby : MonoBehaviour
    {
        public enum LobbyState
        {
            Default,
            JoinedRoom,
        }
        public LobbyState State = LobbyState.Default;
        public bool Debugging = false;

        public GameObject PopoverBackground;
        public GameObject EnterNicknamePopover;
        public GameObject WaitForOpponentPopover;
        public GameObject StartRoomButton;
        public InputField NicknameInputField;

        public GameObject Player1Portrait;
        public GameObject Player2Portrait;

        string nickname;

        private void Start()
        {
            // disable all online UI elements
            HideAllPopover();
            NetworkClient.Lobby.OnLobbyConnectedEvent += OnLobbyConnected();
        }

        void ShowEnterNicknamePopover()
        {
            PopoverBackground.SetActive(true);
            EnterNicknamePopover.SetActive(true);
        }

        void ShowJoinedRoomPopover()
        {
            EnterNicknamePopover.SetActive(false);
            WaitForOpponentPopover.SetActive(true);
            StartRoomButton.SetActive(false);
            Player1Portrait.SetActive(false);
            Player2Portrait.SetActive(false);
        }

        void ShowReadyToStartUI()
        {
            StartRoomButton.SetActive(true);
            Player1Portrait.SetActive(true);
            Player2Portrait.SetActive(true);
        }

        void HideAllPopover()
        {
            PopoverBackground.SetActive(false);
            EnterNicknamePopover.SetActive(false);
            WaitForOpponentPopover.SetActive(false);
            StartRoomButton.SetActive(false);
            Player1Portrait.SetActive(false);
            Player2Portrait.SetActive(false);
        }

        //******************Matchmaking****************************//
        void Checkin()
        {
            NetworkClient.Instance.CheckIn(nickname, (bool successful, string error) =>
            {
                if (!successful)
                {
                    Debug.LogError(error);
                }
            });
        }

        void RegisterToTheLobbyServer()
        {
            NetworkClient.Lobby.Register(nickname, (successful, reply, error) =>
            {
                if (successful)
                {
                    Debug.log("Lobby registered " + reply);
                    if (string.IsNullOrEmpty(reply.roomId))
                    {
                        JoinOrCreateRoom();
                    }
                    else if (reply.started)
                    {
                        State = LobbyState.JoinedRoom;
                        ConnectToRoom();
                    }
                    else
                    {
                        State = LobbyState.JoinedRoom;
                        ShowJoinedRoomPopover();
                        GetPlayersInTheRoom();
                    }
                }
                else
                {
                    Debug.Log("Lobby Failed to register " + reply);
                }
            });
        }

        //******************Lobby Events **************************//
        void OnLobbyConnect()
        {
            RegisterToTheLobbyServer();
        }


        //****************** UI event handlers *********************//
        /// <summary>
        /// Practice button was clicked.
        /// </summary>
        public void OnPracticeClicked()
        {
            Debug.Log("OnPracticeClicked");
            SceneManager.LoadScene("GameScene");
        }

        /// <summary>
        /// Online button was clicked.
        /// </summary>
        public void OnOnlineClicked()
        {
            Debug.Log("OnOnlineClicked");
            ShowEnterNicknamePopover();
        }

        /// <summary>
        /// Cancel button in the popover was clicked.
        /// </summary>
        public void OnCancelClicked()
        {
            Debug.Log("OnCancelClicked");

            if (State == LobbyState.JoinedRoom)
            {
                // TODO: leave room.
            }

            HideAllPopover();
        }

        /// <summary>
        /// Start button in the WaitForOpponentPopover was clicked.
        /// </summary>
        public void OnStartRoomClicked()
        {
            Debug.Log("OnStartRoomClicked");
            // players are ready to player now.
            if (Debugging)
            {
                SceneManager.LoadScene("GameScene");
            }
            else
            {
                // TODO: Start room
            }
        }

        /// <summary>
        /// Ok button in the EnterNicknamePopover was clicked.
        /// </summary>
        public void OnConfirmNicknameClicked()
        {
            nickname = NicknameInputField.text;
            Debug.Log($"OnConfirmNicknameClicked: {nickname}");

            if (Debugging)
            {
                ShowJoinedRoomPopover();
                ShowReadyToStartUI();
            }
            else
            {
                //TODO: Use nickname as player custom id to check into SocketWeaver.
                Checkin();
            }
        }
    }
}
