import { LOGIN, LOGOUT } from ".";

export const login = (data: any) => {
    var accessToken = data.accessToken;
    localStorage.setItem("accessToken", accessToken);
    delete data.accessToken;
    localStorage.setItem("user", JSON.stringify(data));

    return {
        type: LOGIN,
        payload: { ...data, accessToken: accessToken },
    };
};

export const logout = () => {
    localStorage.removeItem("accessToken");
    localStorage.removeItem("user");

    return {
        type: LOGOUT,
        payload: {},
    };
};
