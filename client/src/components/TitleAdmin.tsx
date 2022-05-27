import * as React from "react";

import "styles/TitleAdmin.css";
import { RootStateOrAny, useSelector } from "react-redux";

interface TitleAdminProps {}

const TitleAdmin: React.FC<TitleAdminProps> = () => {
    const user = useSelector((state: RootStateOrAny) => state.user);

    return (
        <div className="titleAdmin w-screen text-white h-28">
            <div className="text-4xl mb-2">
                <b>Welcome back {user.user.name}</b>
            </div>
            <div>Below is admin view</div>
        </div>
    );
};
export default TitleAdmin;
