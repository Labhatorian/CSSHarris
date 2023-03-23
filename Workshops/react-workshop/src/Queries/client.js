import { QueryClient } from 'react-query';

const ONE_MINUTE = 60000;
const queryClient = new QueryClient({
    defaultOptions: {
        queries: {
            refetchOnWindowFocus: false,
            staleTime: ONE_MINUTE
        }
    }
});

export default queryClient;