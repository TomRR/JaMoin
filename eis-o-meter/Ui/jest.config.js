export default {
    preset: "ts-jest",
    testEnvironment: "jsdom", // for React components
    moduleNameMapper: {
        "^@/(.*)$": "<rootDir>/src/$1" // maps '@/' to the src folder
    },
    moduleFileExtensions: ["ts", "tsx", "js", "jsx", "json"],
    testMatch: ["**/?(*.)+(test).[tj]s?(x)"] // looks for *.test.ts / *.test.tsx
};
