import { HubConnectionBuilder } from "@microsoft/signalr";
import {
    JOIN_ROOM,
    LEAVE_ROOM,
    RECEIVE_MESSAGE,
    SEND_MESSAGE,
    RECEIVE_SHAPE,
    DRAW_SHAPE,
    SEND_MOUSE,
    RECEIVE_MOUSE,
    ONLINE_USERS,
    GET_ONLINE_USERS,
    ADD_NOTE,
    UPDATE_NOTE,
    RECEIVE_UPDATE_NOTE,
    RECEIVE_NEW_NOTE,
    DELETE_NOTE,
    RECEIVE_REMOVE_NOTE,
    CLEAR_ALL,
    REMOVE_SHAPE,
    UNDO,
    REDO,
    CONNECT_SIGNALR,
    DISCONNECT_SIGNALR,
    INIT_NOTES,
    INIT_SHAPES,
    LOAD_NOTES,
    LOAD_SHAPES,
} from "store/actions";
import { API } from "utils/constant";
import { Console } from "console";

export const signalRMiddleware = (storeAPI: any) => {
    return (next: any) => async (action: any) => {
        //#region Connect, Join & Leave room

        var connection = storeAPI.getState().connection;

        if (action.type === CONNECT_SIGNALR) {
            if (!connection.chat && !connection.board) {
                connection.board = await createSignalRConnection(
                    `${API}/board`
                );
                connection.chat = await createSignalRConnection(`${API}/chat`);

                action.payload = {
                    board: connection.board,
                    chat: connection.chat,
                };
            }
        }

        if (action.type === DISCONNECT_SIGNALR) {
            try {
                await connection.board.stop();
                await connection.chat.stop();
            } catch (e) {
                console.log(e);
            }
        }

        if (action.type === JOIN_ROOM) {
            await connection.board.invoke("JoinRoom", {
                user: action.payload.user,
                board: action.payload.board,
            });

            await connection.chat.invoke("JoinRoom", {
                user: action.payload.user,
                board: action.payload.board,
            });

            connection.board.on("InitShapes", (shapes: any) => {
                storeAPI.dispatch({
                    type: INIT_SHAPES,
                    payload: shapes,
                });
            });

            connection.chat.on(
                "ReceiveMessage",
                (user: any, message: string, timestamp: Date) => {
                    storeAPI.dispatch({
                        type: RECEIVE_MESSAGE,
                        payload: {
                            user,
                            message,
                            timestamp,
                        },
                    });
                }
            );

            connection.board.on("ReceiveShape", (shape: any) => {
                storeAPI.dispatch({
                    type: RECEIVE_SHAPE,
                    payload: shape,
                });
            });
            connection.board.on("ReceiveBoard", (shape: any) =>{
                const state = storeAPI.getState();
                state.initLC.loadSnapshot(shape);

            })
            
            connection.board.on("ClearAll", () => {
                const state = storeAPI.getState();
                state.initLC.clear();
                state.shape = [];
                state.myShape.undoStack = [];
                state.myShape.redoStack = [];
            })
            


            connection.board.on(
                "ReceiveMouse",
                (
                    userId: string,
                    userName: string,
                    x: number,
                    y: number,
                    isMove: boolean
                ) => {
                    storeAPI.dispatch({
                        type: RECEIVE_MOUSE,
                        payload: {
                            userId,
                            userName,
                            x,
                            y,
                            isMove,
                        },
                    });
                }
            );

            connection.board.on("OnlineUsers", (users: object[]) => {
                storeAPI.dispatch({
                    type: ONLINE_USERS,
                    payload: users,
                });
            });

            connection.board.on("LoadNotes", (notes: any) => {
                storeAPI.dispatch({
                    type: LOAD_NOTES,
                    payload: notes,
                });
            });

            connection.board.on("ReceiveNewNote", (note: Note) => {
                storeAPI.dispatch({
                    type: RECEIVE_NEW_NOTE,
                    payload: note,
                });
            });

            connection.board.on("ReceiveUpdateNote", (note: Note) => {
                storeAPI.dispatch({
                    type: RECEIVE_UPDATE_NOTE,
                    payload: note,
                });
            });

            connection.board.on("ReceiveDeleteNote", (noteId: string) => {
                storeAPI.dispatch({
                    type: RECEIVE_REMOVE_NOTE,
                    payload: noteId,
                });
            });

            connection.board.on("ReceiveUndo", (shapeId: string) => {
                storeAPI.dispatch({
                    type: REMOVE_SHAPE,
                    payload: shapeId,
                });
            });

            connection.chat.onclose(() => {});
            connection.board.onclose(() => {});
        }

        if (action.type === LOAD_SHAPES) {
            connection.board.invoke("LoadInitShapes", action.payload);
        }

        if (action.type === LEAVE_ROOM) {
            connection.board.invoke("LeaveRoom");
            connection.chat.invoke("LeaveRoom");
        }

        //#endregion

        //#region Chat action

        if (action.type === SEND_MESSAGE) {
            connection.chat.invoke(
                "SendMessage",
                action.payload.user,
                action.payload.message
            );
        }

        //#endregion

        //#region Board action

        if (action.type === CLEAR_ALL) {
            connection.board.invoke("ClearAll");
        }

        if (action.type === DRAW_SHAPE) {
            connection.board.invoke("DrawShape", action.payload);
        }

        if (action.type === SEND_MOUSE && connection.board) {
            connection.board.invoke(
                "SendMouse",
                action.payload.x,
                action.payload.y,
                action.payload.isMove
            );
        }

        if (action.type === GET_ONLINE_USERS && connection.board) {
            connection.board.invoke("SendOnlineUsers", action.payload);
        }

        //#endregion

        //#region Note

        if (action.type === INIT_NOTES) {
            connection.board.invoke("LoadNotes", action.payload);
        }

        if (action.type === ADD_NOTE) {
            connection.board.invoke("NewNote", action.payload);
        }

        if (action.type === UPDATE_NOTE) {
            connection.board.invoke("UpdateNote", action.payload);
        }

        if (action.type === DELETE_NOTE) {
            connection.board.invoke("DeleteNote", action.payload);
        }

        //#endregion

        //#region Undo & Redo

        if (action.type === UNDO) {
            const undoStack = storeAPI.getState().myShape.undoStack;
            const lastUndoShape = undoStack[undoStack.length - 1];

            if (lastUndoShape) {
                connection.board.invoke("Undo", lastUndoShape.id);
            }
        }

        if (action.type === REDO) {
            const redoStack = storeAPI.getState().myShape.redoStack;
            const lastRedoShape = redoStack[redoStack.length - 1];

            if (lastRedoShape) {
                connection.board.invoke("Redo", lastRedoShape);
            }
        }

        //#endregion

        return next(action);
    };
};

async function createSignalRConnection(url: string) {
    const newConnection = new HubConnectionBuilder()
        .withUrl(url)
        .withAutomaticReconnect()
        .build();

    await newConnection.start();

    return newConnection;
}
