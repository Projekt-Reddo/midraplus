import {
    ADD_NOTE,
    DELETE_NOTE,
    LOAD_NOTES,
    RECEIVE_NEW_NOTE,
    RECEIVE_REMOVE_NOTE,
    RECEIVE_UPDATE_NOTE,
    UPDATE_NOTE,
} from "store/actions";

const note: Note[] = [];

const noteReducer = (state = note, action: ActionType) => {
    switch (action.type) {
        case ADD_NOTE: {
            return [
                ...state,
                {
                    ...action.payload,
                },
            ];
        }

        case RECEIVE_NEW_NOTE: {
            return [
                ...state,
                {
                    ...action.payload,
                },
            ];
        }

        case DELETE_NOTE: {
            return state.filter((note) => note.id !== action.payload);
        }

        case RECEIVE_REMOVE_NOTE:
            return state.filter((note) => note.id !== action.payload);

        case UPDATE_NOTE: {
            const { id, text } = action.payload;

            return state.map((note) => {
                if (note.id === id) {
                    return {
                        ...note,
                        text,
                    };
                }
                return note;
            });
        }

        case RECEIVE_UPDATE_NOTE: {
            const { id, text } = action.payload;

            return state.map((note) => {
                if (note.id === id) {
                    return {
                        ...note,
                        text,
                    };
                }
                return note;
            });
        }

        case LOAD_NOTES: {
            return action.payload;
        }

        default:
            return state;
    }
};

export default noteReducer;
