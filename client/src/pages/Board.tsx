// Libs
import * as React from "react";
import { useDispatch, useSelector } from "react-redux";
import { useParams } from "react-router-dom";

// Store
import {
    JOIN_ROOM,
    LEAVE_ROOM,
    GET_ONLINE_USERS,
    ONLINE_USERS,
    SEND_MOUSE,
    INIT_NOTES,
    LOAD_SHAPES,
    CLEAR_SHAPE_WHEN_LEAVE_ROOM,
} from "store/actions";

// Components
import CanvasBoard from "components/CanvasBoard";
import Chat from "components/Chat";
import Setting from "components/Setting";
import Avatar from "components/Avatar";

// Styles
import "styles/Board.css";
import Notes from "components/Notes";

interface BoardProps {}

const Board: React.FC<BoardProps> = () => {
    // Lib State
    const dispatch = useDispatch();
    const params: any = useParams();

    // Global State
    const user = useSelector((state: any) => state.user);
    const board = useSelector((state: any) => state.board);
    const connection = useSelector((state: any) => state.connection);

    React.useEffect(() => {
        // User Leave Room
        return () => {
            dispatch({
                type: LEAVE_ROOM,
                payload: null,
            });
            dispatch({
                type: CLEAR_SHAPE_WHEN_LEAVE_ROOM,
                payload: null,
            });
            dispatch({
                type: ONLINE_USERS,
                payload: [],
            });
            dispatch({
                type: SEND_MOUSE,
                payload: {
                    x: 0,
                    y: 0,
                    isMove: false,
                },
            });
        };
    }, []);

    // Get List User Online
    React.useEffect(() => {
        if (board) {
            dispatch({
                type: GET_ONLINE_USERS,
                payload: board,
            });

            dispatch({
                type: INIT_NOTES,
                payload: board,
            });

            dispatch({
                type: LOAD_SHAPES,
                payload: board,
            });
        }
    }, [board]);

    // Join Room
    React.useEffect(() => {
        // Join Board
        if (params.boardId && user.isAuthenticated && connection.board) {
            dispatch({
                type: JOIN_ROOM,
                payload: {
                    user: user.user,
                    board: params.boardId,
                },
            });
        }
    }, [connection]);

    return (
        <>
            <CanvasBoard />
            <Chat />
            <Setting />
            <Avatar />
            <Notes />
        </>
    );
};

export default Board;
