import React, { forwardRef } from "react";
import { Helmet } from "react-helmet";

interface InnerPageProps
  extends React.DetailedHTMLProps<
    React.HTMLAttributes<HTMLDivElement>,
    HTMLDivElement
  > {
  children: React.ReactNode;
  title?: string;
}

function InnerPage(
  { children, title = "", ...rest }: InnerPageProps,
  ref: any
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
