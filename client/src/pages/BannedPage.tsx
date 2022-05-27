import * as React from "react";
import { Navigate, useLocation, useNavigate } from "react-router-dom";

interface BannedPageProps {}

const BannedPage: React.FC<BannedPageProps> = () => {
    const location: any = useLocation();
    let navigate = useNavigate();
    if (!location.state?.isBanned) {
        return <Navigate to="/" />;
    }

    return (
        <div
            className="h-screen w-screen flex items-center justify-center "
            style={{ backgroundColor: "var(--bg)" }}
        >
            <div
                className="h-1/2 w-1/4 flex flex-col items-center justify-around border-rounded app-shadow"
                style={{
                    backgroundColor: "var(--element-bg)",
                    minWidth: "350px",
                    maxWidth: "500px",
                }}
            >
                <div className="basis-2/5 content-center text-center flex flex-col justify-center items-center">
                    <p className="text-3xl text-red-600 font-bold">
                        You are banned
                    </p>
                </div>

                <div className="basis-3/5 flex flex-col items-center justify-start">
                    <p className="text text-white">
                        Please contact our team at <br />
                        <a
                            className="font-bold"
                            href="mailto:draplus.info@gmail.com?subject=Please unban user"
                        >
                            draplus.info@gmail.com
                        </a>
                    </p>
                    <button
                        className="mt-4 bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 border border-blue-700 rounded"
                        onClick={() => {
                            navigate("/");
                        }}
                    >
                        Back to Login Page
                    </button>
                </div>
            </div>
        </div>
    );
};

export default BannedPage;
