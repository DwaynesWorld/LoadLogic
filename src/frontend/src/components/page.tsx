import React, { ForwardedRef, forwardRef } from "react";
import { Helmet } from "react-helmet";

type ReactDiv = React.DetailedHTMLProps<
  React.HTMLAttributes<HTMLDivElement>,
  HTMLDivElement
>;

interface InnerPageProps extends ReactDiv {
  children: React.ReactNode;
  title: string;
}

function InnerPage(
  { children, title = "", ...rest }: InnerPageProps,
  ref: ForwardedRef<any>
) {
  return (
    <div ref={ref} {...rest}>
      <Helmet>
        <title>{title}</title>
      </Helmet>
      {children}
    </div>
  );
}

export const Page = forwardRef(InnerPage);
