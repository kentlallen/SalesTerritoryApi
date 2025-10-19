// src/ErrorBoundary.jsx

import React from 'react';

class ErrorBoundary extends React.Component {
  constructor(props) {
    super(props);
    this.state = { hasError: false, error: null };
  }

  // This lifecycle method is called to update the state so the next render
  // will show the fallback UI.
  static getDerivedStateFromError(error) {
    return { hasError: true, error: error };
  }

  // This lifecycle method is used for logging the error information.
  // In a production app, I would send this to a logging service like Sentry or LogRocket.
  componentDidCatch(error, errorInfo) {
    console.error("Uncaught error:", error, errorInfo);
  }

  render() {
    if (this.state.hasError) {
      return (
        <div className="error-boundary-fallback">
          <h2>Oops! Something went wrong.</h2>
          <p>We're sorry for the inconvenience. Please try refreshing the page.</p>
          <button
            className="btn btn-primary"
            onClick={() => window.location.reload()}
          >
            Refresh Page
          </button>
        </div>
      );
    }

    // If there's no error, just render the children components.
    return this.props.children;
  }
}

export default ErrorBoundary;