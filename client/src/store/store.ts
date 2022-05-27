import { applyMiddleware, createStore, Store } from "redux";
import { signalRMiddleware } from "./middlewares/signalR";
import rootReducer from "./reducers";

type RootState = ReturnType<typeof rootReducer>;

const store: Store<RootState, ActionType> & {
    dispatch: DispatchType;
} = createStore(rootReducer, applyMiddleware(signalRMiddleware));

export default store;
