import jwtDecode from "jwt-decode";

export const init = () => {
    // const dispatch = useDispatch();
    try {
        const accessToken = localStorage.getItem("accessToken");
        if (accessToken) {
            const decodedToken: any = jwtDecode(accessToken);
            if (decodedToken.exp * 1000 < Date.now()) {
                localStorage.removeItem("accessToken");
                localStorage.removeItem("user");
                console.log("Token expired");
                return false;
            }
            console.log("Token valid");
            return true;
        }
    } catch (error) {
        console.error(error);
    }
    return false;
};
