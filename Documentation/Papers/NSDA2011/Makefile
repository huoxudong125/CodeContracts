NAME=paper
COMMON=

$(NAME).pdf: $(NAME).tex $(COMMON) $(NAME).bib
	pdflatex $(NAME)
  bibtex $(NAME)
	pdflatex $(NAME)
	pdflatex $(NAME)

edit:
	sd edit $(NAME).tex
	sd edit $(NAME).bib
	runemacs $(NAME).tex

view: $(NAME).pdf
	$(NAME).pdf

clean:
	del $(NAME).pdf
	del $(NAME).log
  del $(NAME).aux
  del *~

