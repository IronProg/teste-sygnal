import {render, screen} from '@testing-library/react';
import userEvent from "@testing-library/user-event";
import Navbar from "./Navbar";

test('navbar has logo', async () => {
    render(<Navbar />);

    const logo = screen.getByAltText(/sygnalgroup logo/i);
    expect(logo).toBeInTheDocument();
});
