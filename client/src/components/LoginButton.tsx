import * as React from "react";
import { GOOGLE_CLIENT_ID } from "utils/constant";
import { GoogleLogin } from "react-google-login";

import "styles/GoogleLoginButton.css";

interface LoginButtonProps {
    googleResponse: () => void;
}
// interface IAuthResponse {
//     token: string;
// }
// type ResponseUser = {
//     user: JSON;
// };
// type queryType = {
//     manual: boolean;
// };
const LoginButton: React.FC<LoginButtonProps> = ({ googleResponse }) => {
    const styles = { borderRadius: 50, width: "100%" };

    return (
        <div className="content-center justify-center my-2">
            <GoogleLogin
                clientId={GOOGLE_CLIENT_ID}
                buttonText="Google Login"
                render={(renderProps) => (
                    <button onClick={renderProps.onClick} style={styles}>
                        <div className="google-btn border-rounded hover:bg-blue-600">
                            <div className="google-icon-wrapper border-rounded">
                                <img
                                    className="google-icon"
                                    src="https://upload.wikimedia.org/wikipedia/commons/5/53/Google_%22G%22_Logo.svg"
                                    alt="google logo"
                                />
                            </div>
                            <div className="w-full h-full">
                                <p className="btn-text border-rounded mr-6 mt-[0.6rem]">
                                    Sign in with google
                                </p>
                            </div>
                        </div>
                    </button>
                )}
                onSuccess={googleResponse}
                onFailure={googleResponse}
                style={styles}
            />
        </div>
    );
};

export default LoginButton;
