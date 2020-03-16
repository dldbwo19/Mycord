using System;
using System.Collections.Generic;
using System.Text;

namespace Mycord_WPF
{
    public enum EProtocol : short
    {
        BEGIN = 0,

        SET_NAME_REQ,
        SET_NAME_ACK,
        SET_NAME_ERROR_EXISTED,

        // 유저 리스트 요청 (클라이언트 -> 서버 -> 클라이언트)
        ASK_USER_LIST_REQ,
        ASK_USER_LIST_ACK,
        ASK_USER_LIST_ERROR,

        // 채팅방 생성 요청 (클라이언트 -> 서버)
        CREATE_CHAT_ROOM_REQ,
        CREATE_CHAT_ROOM_ERROR,
        CREATE_CHAT_ROOM_ACK,

        //
        JOIN_THE_ROOM_REQ,
        JOIN_THE_ROOM_ACK,
        JOIN_THE_ROOM_ERROR,
        JOIN_THE_ROOM_NEW_USER,

        // 채팅방 로딩을 시작 (서버 -> 클라이언트)
        START_LOADING,
        // 로딩 완료 (클라이언트 -> 서버)
        LOADING_COMPLETED_REQ,
        // 채팅 시작 (서버 -> 클라이언트)
        CHAT_START,

        //
        DESTROYED_CHAT_ROOM,

        // 채팅 전송 요청 (클라이언트 -> 서버)
        CHAT_MSG_REQ,
        // 채팅 전송 완료 (서버 -> 클라이언트)
        CHAT_MSG_ACK,

        // 유저 방 초대 요청 (클라이언트 -> 서버)
        ADD_USER_IN_ROOM_REQ,
        ADD_USER_IN_ROOM_ACK,
        ADD_USER_IN_ROOM_ERROR,

        // 유저 방 나가기 요청 (클라이언트 -> 서버)
        EXIT_THE_ROOM_REQ,
        EXIT_THE_ROOM_ACK,
        EXIT_THE_ROOM_USER,
        EXIT_THE_ROOM_ERROR,

        DISCONNECTION_REQ,

        END
    }
}
