import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import './index.css'
import {createBrowserRouter, RouterProvider} from "react-router";
import App from './App.tsx'
import NotFoundPage from "@/feature/notfound/NotFoundPage.tsx";
import IceRewardPage from "@/feature/ice-reward/IceRewardPage.tsx";

const router = createBrowserRouter([
    {
        path: "/",
        element: <App />,
        children: [
            { index: true, element: <IceRewardPage /> },
        ]
    },
    {path:"*", element:<NotFoundPage/> },
]);

createRoot(document.getElementById('root')!).render(
    <StrictMode>
            <RouterProvider router={router} />
    </StrictMode>
)
