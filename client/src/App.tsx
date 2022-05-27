import "./App.css";
import { BrowserRouter as Router } from "react-router-dom";
import { QueryClient, QueryClientProvider } from "react-query";

import BaseRoutes from "routes";
import { useDispatch, useSelector } from "react-redux";
import { useEffect } from "react";
import { CONNECT_SIGNALR } from "store/actions";

function App() {
    const dispatch = useDispatch();

    const user = useSelector((state: any) => state.user);

    useEffect(() => {
        if (user.isAuthenticated) {
            dispatch({
                type: CONNECT_SIGNALR,
            });
        }
    }, [user]);

    return (
        <QueryClientProvider client={new QueryClient()}>
            <Router>
                <BaseRoutes />
            </Router>
        </QueryClientProvider>
    );
}

export default App;
