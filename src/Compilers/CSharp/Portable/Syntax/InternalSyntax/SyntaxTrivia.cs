﻿// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using Roslyn.Utilities;

namespace Microsoft.CodeAnalysis.CSharp.Syntax.InternalSyntax
{
    internal class SyntaxTrivia : CSharpSyntaxNode
    {
        public readonly string Text;

        internal SyntaxTrivia(SyntaxKind kind, string text, DiagnosticInfo[] diagnostics = null, SyntaxAnnotation[] annotations = null)
            : base(kind, diagnostics, annotations, text.Length)
        {
            this.Text = text;
            if (kind == SyntaxKind.PreprocessingMessageTrivia)
            {
                this.flags |= NodeFlags.ContainsSkippedText;
            }
        }

        internal SyntaxTrivia(ObjectReader reader)
            : base(reader)
        {
            this.Text = reader.ReadString();
            this.FullWidth = this.Text.Length;
        }

        internal override Func<ObjectReader, object> GetReader()
        {
            return r => new SyntaxTrivia(r);
        }

        internal override void WriteTo(ObjectWriter writer)
        {
            base.WriteTo(writer);
            writer.WriteString(this.Text);
        }

        internal static SyntaxTrivia Create(SyntaxKind kind, string text)
        {
            return new SyntaxTrivia(kind, text);
        }

        public override string ToFullString()
        {
            return this.Text;
        }

        public override string ToString()
        {
            return this.Text;
        }

        internal override GreenNode GetSlot(int index)
        {
            throw ExceptionUtilities.Unreachable;
        }

        public override int Width
        {
            get
            {
                Debug.Assert(this.FullWidth == this.Text.Length);
                return this.FullWidth;
            }
        }

        public override int GetLeadingTriviaWidth()
        {
            return 0;
        }

        public override int GetTrailingTriviaWidth()
        {
            return 0;
        }

        internal override GreenNode SetDiagnostics(DiagnosticInfo[] diagnostics)
        {
            return new SyntaxTrivia(this.Kind, this.Text, diagnostics, GetAnnotations());
        }

        internal override GreenNode SetAnnotations(SyntaxAnnotation[] annotations)
        {
            return new SyntaxTrivia(this.Kind, this.Text, GetDiagnostics(), annotations);
        }

        public override TResult Accept<TResult>(CSharpSyntaxVisitor<TResult> visitor)
        {
            return visitor.VisitTrivia(this);
        }

        public override void Accept(CSharpSyntaxVisitor visitor)
        {
            visitor.VisitTrivia(this);
        }

        protected internal override void WriteTo(System.IO.TextWriter writer, bool leading, bool trailing)
        {
            writer.Write(Text);
        }

        public static implicit operator Microsoft.CodeAnalysis.SyntaxTrivia(SyntaxTrivia trivia)
        {
            return new Microsoft.CodeAnalysis.SyntaxTrivia(default(Microsoft.CodeAnalysis.SyntaxToken), trivia, position: 0, index: 0);
        }

        public override bool IsEquivalentTo(GreenNode other)
        {
            if (!base.IsEquivalentTo(other))
            {
                return false;
            }

            if (this.Text != ((SyntaxTrivia)other).Text)
            {
                return false;
            }

            return true;
        }

        internal override SyntaxNode CreateRed(SyntaxNode parent, int position)
        {
            throw ExceptionUtilities.Unreachable;
        }
    }
}
