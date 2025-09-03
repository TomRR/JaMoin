export const getAuthToken = async (): Promise<string> => {

    const token = await window.Clerk?.session?.getToken();
    // const token = await Clerk.session.getToken(); // 👈 Clerk's session token
    if (!token) {
        throw new Error("No Clerk token available");
    }

    return token;
};

declare global {
    interface Window {
        Clerk: any;
    }
}
