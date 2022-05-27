type ActionType = {
    type: string;
    payload?: any;
};

type DispatchType = (args: ActionType) => ActionType;
